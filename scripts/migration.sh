#!/bin/bash
set -e

# Colors
CYAN='\033[0;36m'
YELLOW='\033[1;33m'
GREEN='\033[0;32m'
RED='\033[0;31m'
NC='\033[0m'

# Parse arguments
ACTION=""
NAME=""

show_usage() {
    echo "Usage: ./migration.sh <action> [name]"
    echo ""
    echo "Actions:"
    echo "  add <name>    Create a new migration"
    echo "  remove        Remove the last migration"
    echo "  list          List all migrations"
    echo "  update        Apply migrations to database"
}

if [ $# -eq 0 ]; then
    show_usage
    exit 1
fi

ACTION=$1
shift

case $ACTION in
    add)
        if [ $# -eq 0 ]; then
            echo -e "${RED}❌ Migration name is required for 'add' action${NC}"
            show_usage
            exit 1
        fi
        NAME=$1
        ;;
    remove|list|update)
        ;;
    *)
        echo -e "${RED}❌ Unknown action: $ACTION${NC}"
        show_usage
        exit 1
        ;;
esac

SCRIPT_DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )"
INFRA_PROJECT="$SCRIPT_DIR/../src/EastSeat.ResourceIdea.Infrastructure"
SERVER_PROJECT="../EastSeat.ResourceIdea.Server"

cd "$INFRA_PROJECT"

case $ACTION in
    add)
        echo -e "${CYAN}➕ Creating migration: $NAME${NC}"
        dotnet ef migrations add "$NAME" --startup-project "$SERVER_PROJECT" --context ApplicationDbContext
        echo -e "${GREEN}✅ Migration created successfully${NC}"
        ;;
    remove)
        echo -e "${CYAN}➖ Removing last migration...${NC}"
        dotnet ef migrations remove --startup-project "$SERVER_PROJECT" --context ApplicationDbContext
        echo -e "${GREEN}✅ Migration removed successfully${NC}"
        ;;
    list)
        echo -e "${CYAN}📋 Listing migrations...${NC}"
        dotnet ef migrations list --startup-project "$SERVER_PROJECT" --context ApplicationDbContext
        ;;
    update)
        echo -e "${CYAN}🔄 Updating database...${NC}"
        dotnet ef database update --startup-project "$SERVER_PROJECT" --context ApplicationDbContext
        echo -e "${GREEN}✅ Database updated successfully${NC}"
        ;;
esac
