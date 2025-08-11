# EasyBook — Local AI Setup

<!-- This README documents how to run a local LLM via Ollama for EasyBook. 
It uses Qwen 2.5 7B Instruct as the default model. -->

## Prerequisites
- Docker Desktop installed and running
- ~5 GB free disk space
- Port `11434` available

## Quick Start

```bash
# 1) Create & start the Ollama container (persists models in a named volume "ollama")
docker run -d --name ollama --restart unless-stopped \
  -p 11434:11434 -v ollama:/root/.ollama ollama/ollama

# 2) Download the model (Qwen 2.5 7B Instruct)
docker exec -it ollama ollama pull qwen2.5:7b-instruct

# 3) (Optional) Open an interactive chat session in the container
# docker exec -it ollama ollama run qwen2.5:7b-instruct
