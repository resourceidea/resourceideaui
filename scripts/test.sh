#!/bin/bash
set -e

# Colors
CYAN='\033[0;36m'
YELLOW='\033[1;33m'
GREEN='\033[0;32m'
RED='\033[0;31m'
NC='\033[0m'

echo -e "${CYAN}🧪 Running tests...${NC}"

# Parse arguments
COVERAGE=false
WATCH=false

while [[ $# -gt 0 ]]; do
    case $1 in
        --coverage)
            COVERAGE=true
            shift
            ;;
        --watch)
            WATCH=true
            shift
            ;;
        *)
            echo -e "${RED}Unknown option: $1${NC}"
            exit 1
            ;;
    esac
done

SCRIPT_DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )"
SOLUTION_PATH="$SCRIPT_DIR/../EastSeat.ResourceIdea.slnx"

if [ "$WATCH" = true ]; then
    echo -e "${YELLOW}Running in watch mode. Press Ctrl+C to stop.${NC}"
    dotnet watch test "$SOLUTION_PATH" --verbosity normal
elif [ "$COVERAGE" = true ]; then
    echo -e "${YELLOW}Generating coverage report...${NC}"
    dotnet test "$SOLUTION_PATH" \
        --collect:"XPlat Code Coverage" \
        --results-directory:"./coverage" \
        --verbosity normal
    
    echo -e "${GREEN}✅ Coverage report generated in ./coverage directory${NC}"
else
    dotnet test "$SOLUTION_PATH" --verbosity normal
fi

if [ $? -eq 0 ]; then
    echo -e "${GREEN}✅ All tests passed!${NC}"
else
    echo -e "${RED}❌ Some tests failed!${NC}"
    exit 1
fi
