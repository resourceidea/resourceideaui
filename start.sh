#!/usr/bin/env bash
set -euo pipefail
if [ ! -f .env ]; then
  cp .env.example .env
  echo "Created .env from example"
fi
export $(grep -v '^#' .env | xargs)
cd docker
docker compose build
docker compose up -d
echo "Containers started. Server at http://localhost:5001, Client at http://localhost:8080"
