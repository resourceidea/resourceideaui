#!/bin/bash
set -e

# Colors
CYAN='\033[0;36m'
YELLOW='\033[1;33m'
GREEN='\033[0;32m'
GRAY='\033[0;37m'
NC='\033[0m'

echo -e "${CYAN}🧹 Cleaning build artifacts...${NC}"

# Parse arguments
ALL=false
if [ "$1" = "--all" ]; then
    ALL=true
fi

SCRIPT_DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )"
cd "$SCRIPT_DIR/.."

# Clean solution manifest (using .slnx)
dotnet clean "./EastSeat.ResourceIdea.slnx"

# Remove bin and obj directories
echo -e "${GRAY}Removing bin and obj directories...${NC}"
find . -type d \( -name "bin" -o -name "obj" \) -exec rm -rf {} + 2>/dev/null || true

# Remove test results
if [ -d "coverage" ]; then
    echo -e "${GRAY}Removing coverage reports...${NC}"
    rm -rf coverage
fi

if [ -d "TestResults" ]; then
    echo -e "${GRAY}Removing test results...${NC}"
    rm -rf TestResults
fi

if [ "$ALL" = true ]; then
    echo -e "${YELLOW}🗑️  Performing deep clean...${NC}"
    
    # Stop and remove Docker containers
    echo -e "${GRAY}Stopping Docker containers...${NC}"
    if docker-compose down -v 2>/dev/null; then
        echo -e "${GREEN}✅ Docker containers and volumes removed${NC}"
    else
        echo -e "${YELLOW}⚠️  Failed to remove Docker containers (may not be running)${NC}"
    fi
fi

echo -e "${GREEN}✅ Clean completed!${NC}"
