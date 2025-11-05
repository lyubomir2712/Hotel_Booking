# EasyBook - AI Powered HotelBooking Web Application  

**ASP.NET Core Web Application with integrated AI chatbot.**  
The project follows a clean **N-tier architecture** with **Unit of Work** and the **Repository pattern**.  
It integrates with external services such as the **Booking.com API** and provides full **CRUD operations** for hotel data and reservations.  

### Key Features  
- **User Experience**: personal bookings cart, favourites page, responsive design across all devices.  
- **Identity & Security**: built-in Identity UI for authentication and authorization.  
- **Admin Panel**: monitors all bookings with detailed data and ordering options.  
- **Real-Time Updates**: SignalR with WebSocket connections for instant admin notifications.  
- **Event-Driven System**: integrated Kafka for logging and handling client operations.  
- **Email Notifications**: automated email sender using **MailKit** for new user registrations and cart checkouts.  
- **Testing & Quality**: comprehensive unit and integration tests using **xUnit** and **Moq**.  
- **Containerization**: Dockerized SQL database, Ollama client, Redis and Kafka services.  
- **In-Memory Database (Redis)**: a lightweight in-memory layer backed by Redis used to store Kafka logging events temporarily for quick lookups and monitoring.  
- **AI Chat Bot (Ollama + Owen)**: integrated local AI assistant powered by **Ollama** and **Owen**, used for intelligent chat interactions, hotel recommendations, and automated support directly within the application interface.

## 🛠 Tech Stack  

<p align="center">
  <img src="https://upload.wikimedia.org/wikipedia/commons/e/ee/.NET_Core_Logo.svg" alt="ASP.NET Core" width="60" height="60"/>
  <img src="https://upload.wikimedia.org/wikipedia/commons/4/4f/Csharp_Logo.png" alt="C#" width="60" height="60"/>
  <img src="https://github.com/user-attachments/assets/10541c91-be97-4ac5-b031-19907ff3e54f" height="60" width="60" alt="Entity Framework"/>
  <img src="https://www.svgrepo.com/show/303229/microsoft-sql-server-logo.svg" alt="SQL Server" width="60" height="60"/>
  <img src="https://icon.icepanel.io/Technology/svg/Apache-Kafka.svg" alt="Kafka" width="60" height="60"/>
  <img src="https://github.com/user-attachments/assets/c0672c53-68e7-4bb0-bb9f-41fadd727a4d" alt="SignalR" height="60" width="60"/>
  <img src="https://github.com/user-attachments/assets/a5ba288a-2e2a-4e58-bd39-0e57de4fe4da" alt="XUnit" height="60" width="60"/>
  <img src="https://img.favpng.com/5/21/1/docker-logo-kubernetes-microservices-cloud-computing-png-favpng-qZv8eQ1wcWx99NuZ6NB8HHHmk.jpg" alt="Docker" height="60" width="60"/>
  <img src="https://api.nuget.org/v3-flatcontainer/moq/4.20.72/icon" alt="Moq" width="60" height="60"/>
  <img src="https://ih1.redbubble.net/image.5611428487.0532/st,small,507x507-pad,600x600,f8f8f8.jpg" alt="Ollama" width="60" height="60"/>
  <img src="https://github.com/user-attachments/assets/63aa2ba4-cde5-4914-9dff-e1f36fbce961" alt="MailKit" width="60" height="60"/>
  <img alt="HTML" height="60" width="60" src="https://cdn.jsdelivr.net/gh/devicons/devicon@latest/icons/html5/html5-original.svg"/> 
  <img alt="CSS" height="60" width="60" src="https://cdn.jsdelivr.net/gh/devicons/devicon@latest/icons/css3/css3-original.svg"/> 
  <img alt="JavaScript" height="60" width="60" src="https://cdn.jsdelivr.net/gh/devicons/devicon@latest/icons/javascript/javascript-original.svg"/> 
  <img alt="Redis" height="60" width="60" src="https://cdn.jsdelivr.net/gh/devicons/devicon@latest/icons/redis/redis-original.svg"/>
</p>

## EasyBook — Local AI Setup

<!-- This README documents how to run a local LLM via Ollama for EasyBook. 
It uses Qwen 2.5 7B Instruct as the default model. -->

### Prerequisites
- Docker Desktop installed and running
- ~5 GB free disk space
- Port `11434` available

### Quick Start

## 🦙 Ollama/Qwen AI Setup  
```bash
# 1) Create & start the Ollama container (persists models in a named volume "ollama")
docker run -d --name ollama --restart unless-stopped \
  -p 11434:11434 -v ollama:/root/.ollama ollama/ollama

# 2) Download the model (Qwen 2.5 7B Instruct)
docker exec -it ollama ollama pull qwen2.5:7b-instruct

# 3) (Optional) Open an interactive chat session in the container
# docker exec -it ollama ollama run qwen2.5:7b-instruct
```

## 🧠 Redis Setup  
```bash 
docker run -d --name redis -p 6379:6379 redis:latest
```

## 🛠️ Kafka Setup

```bash
# 1) Creating the Kafka Cluster on Docker container
docker volume create kafka_data

docker run -d \
  --name kafka \
  --hostname kafka \
  -p 9092:9092 \
  -e KAFKA_ENABLE_KRAFT=yes \
  -e KAFKA_CFG_NODE_ID=1 \
  -e KAFKA_CFG_PROCESS_ROLES=broker,controller \
  -e KAFKA_CFG_CONTROLLER_LISTENER_NAMES=CONTROLLER \
  -e KAFKA_CFG_CONTROLLER_QUORUM_VOTERS=1@kafka:9093 \
  -e KAFKA_CFG_LISTENERS=PLAINTEXT://:9092,CONTROLLER://:9093 \
  -e KAFKA_CFG_ADVERTISED_LISTENERS=PLAINTEXT://localhost:9092 \
  -e KAFKA_CFG_LISTENER_SECURITY_PROTOCOL_MAP=PLAINTEXT:PLAINTEXT,CONTROLLER:PLAINTEXT \
  -e KAFKA_CFG_INTER_BROKER_LISTENER_NAME=PLAINTEXT \
  -e KAFKA_CFG_OFFSETS_TOPIC_REPLICATION_FACTOR=1 \
  -e KAFKA_CFG_TRANSACTION_STATE_LOG_REPLICATION_FACTOR=1 \
  -e KAFKA_CFG_TRANSACTION_STATE_LOG_MIN_ISR=1 \
  -e KAFKA_CFG_GROUP_INITIAL_REBALANCE_DELAY_MS=0 \
  -e KAFKA_CFG_LOG_DIRS=/bitnami/kafka/data \
  -e ALLOW_PLAINTEXT_LISTENER=yes \
  -e BITNAMI_DEBUG=true \
  --health-cmd="bash -c 'kafka-topics.sh --bootstrap-server localhost:9092 --list >/dev/null 2>&1'" \
  --health-interval=10s \
  --health-timeout=5s \
  --health-retries=12 \
  -v kafka_data:/bitnami/kafka \
  --restart unless-stopped \
  bitnami/kafka:latest

# 2) Creating the topic for the operations logger
docker exec -it kafka kafka-topics.sh \
  --create \
  --topic ops-log \
  --bootstrap-server localhost:9092 \
  --partitions 12 \
  --replication-factor 1
```
## 💿 SqlServer Setup 
In HotelBooking.Web replace
```
ConnectionStrings__DefaultConnection;
ConnectionStrings__BookingDbContextConnection;
``` 
in the .env(which i provided for easier setup, i'll be glad if you don't waste my api key 🥹 ) with your connection string, or if you intend not to use the .env you can directly put them in the appsettings.json and remove Env.Load() at the start of Program.cs
```
"DefaultConnection": "",
    "BookingDbContextConnection": "",
```
    
## 📸 Screenshots  

### Home Page  
<img width="1728" height="1080" alt="Screenshot 2025-11-05 at 8 27 41" src="https://github.com/user-attachments/assets/149c2699-cc1c-410e-8f1c-faefc689c62c" />
<img width="1728" height="1080" alt="Screenshot 2025-11-05 at 8 29 57" src="https://github.com/user-attachments/assets/648c3a94-1fb6-488e-9f9e-a8df3e832bf8" />

### AI 
<img width="1728" height="1080" alt="Screenshot 2025-11-05 at 8 28 58" src="https://github.com/user-attachments/assets/861c9e04-a84d-456c-9ec7-2f527ce0df23" />

### Hotel Search
<img width="1728" height="1080" alt="Screenshot 2025-11-05 at 8 47 20" src="https://github.com/user-attachments/assets/ea99c80d-faf1-4506-936f-d6417ffab112" />


### Admin Panel
<img width="1728" height="1080" alt="Screenshot 2025-11-05 at 8 30 42" src="https://github.com/user-attachments/assets/d8ce9387-c4ee-4099-85f1-bc7ef24fef06" />

### Booking Cart  
<img src="screenshots/cart.png" alt="Booking Cart" width="800">  
<img width="1728" height="1080" alt="Screenshot 2025-11-05 at 8 58 19" src="https://github.com/user-attachments/assets/55b06f28-150d-4151-9e07-b3327004a37d" />

### Admin Panel  
<img src="screenshots/admin.png" alt="Admin Panel" width="800">  

### Emails
<img width="1728" height="1080" alt="Screenshot 2025-11-05 at 8 59 30" src="https://github.com/user-attachments/assets/c2363432-d240-4c4c-bb4a-f43ad474c2c0" />
<img width="1728" height="1080" alt="Screenshot 2025-11-05 at 8 59 40" src="https://github.com/user-attachments/assets/c6689082-0165-401d-9e0f-90483753f2a4" />

### Redis
<img width="777" height="570" alt="Screenshot 2025-11-05 at 9 03 12" src="https://github.com/user-attachments/assets/3abca909-045a-42fa-8c83-3404377c34c4" />

### Docker
<img width="1382" height="832" alt="Screenshot 2025-11-05 at 9 02 44" src="https://github.com/user-attachments/assets/60e58b6c-8719-4ec5-8bde-03c3d00cb4c9" />

### Database diagram
<img width="1728" height="1080" alt="Screenshot 2025-11-05 at 9 05 00" src="https://github.com/user-attachments/assets/7b4a56a5-a7c4-4fcc-8fb2-b8505d1b00bd" />

### SignalR notifications
![SignalRNotifications](https://github.com/user-attachments/assets/395e1c83-d85c-4dfc-b74a-4a02c4a36812)







