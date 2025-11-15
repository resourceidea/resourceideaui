#!/bin/bash
set -e

# Colors
CYAN='\033[0;36m'
YELLOW='\033[1;33m'
RED='\033[0;31m'
NC='\033[0m'

echo -e "${CYAN}🔥 Starting ResourceIdea in watch mode (hot reload enabled)...${NC}"
echo ""
echo -e "${YELLOW}The application will automatically reload when you save changes to .cs or .razor files${NC}"
echo ""

# Get script directory
SCRIPT_DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )"
SERVER_PATH="$SCRIPT_DIR/../src/EastSeat.ResourceIdea.Server"

if [ ! -d "$SERVER_PATH" ]; then
    echo -e "${RED}❌ Server project not found at: $SERVER_PATH${NC}"
    exit 1
fi

cd "$SERVER_PATH"
dotnet watch run
