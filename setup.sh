#!/bin/bash
set -e

# Colors for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
CYAN='\033[0;36m'
NC='\033[0m' # No Color

info() {
    echo -e "${CYAN}ℹ️  $1${NC}"
}

success() {
    echo -e "${GREEN}✅ $1${NC}"
}

error() {
    echo -e "${RED}❌ $1${NC}"
}

warning() {
    echo -e "${YELLOW}⚠️  $1${NC}"
}

# Parse arguments
SKIP_DOCKER=false
SKIP_MIGRATIONS=false
SKIP_BUILD=false
PRODUCTION=false

while [[ $# -gt 0 ]]; do
    case $1 in
        --skip-docker)
            SKIP_DOCKER=true
            shift
            ;;
        --skip-migrations)
            SKIP_MIGRATIONS=true
            shift
            ;;
        --skip-build)
            SKIP_BUILD=true
            shift
            ;;
        --production)
            PRODUCTION=true
            shift
            ;;
        --help)
            echo "Usage: ./setup.sh [OPTIONS]"
            echo ""
            echo "Options:"
            echo "  --skip-docker       Skip starting Docker containers"
            echo "  --skip-migrations   Skip running database migrations"
            echo "  --skip-build        Skip building the solution"
            echo "  --production        Run in production mode"
            echo "  --help              Show this help message"
            exit 0
            ;;
        *)
            error "Unknown option: $1"
            echo "Use --help for usage information"
            exit 1
            ;;
    esac
done

cat << "EOF"
╔════════════════════════════════════════════════════════════╗
║         EastSeat ResourceIdea - Setup Script               ║
║                                                            ║
║  Resource Planning for Audit & Tax Advisory Firms         ║
╚════════════════════════════════════════════════════════════╝

EOF

# Check prerequisites
info "Checking prerequisites..."

# Check .NET SDK
if command -v dotnet &> /dev/null; then
    DOTNET_VERSION=$(dotnet --version)
    success ".NET SDK found: $DOTNET_VERSION"
else
    error ".NET SDK not found. Please install .NET 9 SDK from https://dotnet.microsoft.com/download"
    exit 1
fi

# Check Docker (unless skipped)
if [ "$SKIP_DOCKER" = false ]; then
    if command -v docker &> /dev/null; then
        DOCKER_VERSION=$(docker --version)
        success "Docker found: $DOCKER_VERSION"
    else
        warning "Docker not found. Use --skip-docker if you have an existing PostgreSQL instance."
        read -p "Continue without Docker? (y/n) " -n 1 -r
        echo
        if [[ ! $REPLY =~ ^[Yy]$ ]]; then
            exit 1
        fi
        SKIP_DOCKER=true
    fi
fi

# Set environment
if [ "$PRODUCTION" = true ]; then
    export ASPNETCORE_ENVIRONMENT="Production"
else
    export ASPNETCORE_ENVIRONMENT="Development"
fi
info "Environment: $ASPNETCORE_ENVIRONMENT"

# Load environment variables from .env if exists
if [ -f .env ]; then
    info "Loading environment variables from .env file..."
    while IFS='=' read -r key value; do
        # Skip comments and empty lines
        if [[ ! $key =~ ^# && -n $key ]]; then
            # Remove leading/trailing whitespace
            key=$(echo "$key" | xargs)
            value=$(echo "$value" | xargs)
            export "$key=$value"
            success "Loaded: $key"
        fi
    done < .env
else
    warning ".env file not found. Creating from .env.sample..."
    if [ -f .env.sample ]; then
        cp .env.sample .env
        success "Created .env file. Please review and update the configuration."
        warning "Default password will be used for admin account: Admin@123"
    else
        error ".env.sample not found!"
        exit 1
    fi
fi

# Start Docker containers
if [ "$SKIP_DOCKER" = false ]; then
    info "Starting PostgreSQL container..."
    if docker-compose up -d; then
        success "Docker containers started"
        
        # Wait for PostgreSQL to be ready
        info "Waiting for PostgreSQL to be ready..."
        MAX_ATTEMPTS=30
        ATTEMPT=0
        DB_READY=false
        
        while [ $ATTEMPT -lt $MAX_ATTEMPTS ] && [ "$DB_READY" = false ]; do
            ATTEMPT=$((ATTEMPT + 1))
            if docker-compose exec -T postgres pg_isready -U postgres &> /dev/null; then
                DB_READY=true
                success "PostgreSQL is ready!"
            else
                echo -n "."
                sleep 2
            fi
        done
        echo ""
        
        if [ "$DB_READY" = false ]; then
            error "PostgreSQL failed to start within expected time"
            exit 1
        fi
    else
        error "Failed to start Docker containers"
        exit 1
    fi
fi

# Restore and build solution
if [ "$SKIP_BUILD" = false ]; then
    MANIFEST="EastSeat.ResourceIdea.slnx"
    SLN_FILE="EastSeat.ResourceIdea.slnx"
    if [ ! -f "$MANIFEST" ]; then
        error "XML manifest '$MANIFEST' not found. Cannot generate solution."
        exit 1
    fi
    info "Using .slnx manifest directly (generation of .sln skipped)"
    success "Manifest ready: $SLN_FILE"

    info "Restoring NuGet packages..."
    if dotnet restore "./$SLN_FILE"; then
        success "Packages restored"
    else
        error "Failed to restore packages"
        exit 1
    fi

    info "Building solution..."
    if [ "$PRODUCTION" = true ]; then
        BUILD_CONFIG="Release"
    else
        BUILD_CONFIG="Debug"
    fi
    if dotnet build "./$SLN_FILE" --configuration "$BUILD_CONFIG" --no-restore; then
        success "Build completed"
    else
        error "Build failed"
        exit 1
    fi
fi

# Run database migrations
if [ "$SKIP_MIGRATIONS" = false ]; then
    info "Checking for EF Core tools..."
    
    # Check if migrations exist
    MIGRATIONS_PATH="src/EastSeat.ResourceIdea.Infrastructure/Migrations"
    if [ ! -d "$MIGRATIONS_PATH" ] || [ -z "$(ls -A "$MIGRATIONS_PATH" 2>/dev/null)" ]; then
        info "Creating initial migration..."
        cd src/EastSeat.ResourceIdea.Infrastructure
        if dotnet ef migrations add InitialCreate --startup-project ../EastSeat.ResourceIdea.Server --context ApplicationDbContext; then
            cd ../..
            success "Initial migration created"
        else
            cd ../..
            error "Failed to create migration"
            exit 1
        fi
    else
        info "Migrations already exist"
    fi

    info "Applying database migrations..."
    cd src/EastSeat.ResourceIdea.Infrastructure
    if dotnet ef database update --startup-project ../EastSeat.ResourceIdea.Server --context ApplicationDbContext; then
        cd ../..
        success "Database updated"
    else
        cd ../..
        error "Migration failed"
        warning "If database doesn't exist, ensure PostgreSQL is running and connection string is correct."
        exit 1
    fi
fi

# Run tests
info "Running tests..."
if dotnet test "./$SLN_FILE" --no-build --verbosity quiet; then
    success "All tests passed"
else
    warning "Some tests failed. Check output above for details."
fi

# Start application
cat << "EOF"

╔════════════════════════════════════════════════════════════╗
║                   Starting Application                     ║
╚════════════════════════════════════════════════════════════╝

EOF

info "Starting ResourceIdea..."
echo ""
echo -e "${CYAN}Application will be available at:${NC}"
echo -e "${YELLOW}  • https://localhost:7001${NC}"
echo -e "${YELLOW}  • http://localhost:5001${NC}"
echo ""
echo -e "${CYAN}Default admin credentials:${NC}"
echo -e "${YELLOW}  • Email: admin@eastseat.com${NC}"
echo -e "${YELLOW}  • Password: Admin@123 (or from ADMIN_PASSWORD env)${NC}"
echo ""
echo -e "Press Ctrl+C to stop the application"
echo ""

cd src/EastSeat.ResourceIdea.Server
if [ "$PRODUCTION" = true ]; then
    dotnet run --configuration Release --no-build
else
    dotnet run --configuration Debug --no-build
fi
