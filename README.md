# EasyBook - AI Powered HotelBooking Web Application  

**ASP.NET Core Web Application with integrated AI chatbot.**  
The project follows a clean **N-tier architecture** with **Unit of Work** and the **Repository pattern**.  
It integrates with external services such as the **Booking.com API** and provides full **CRUD operations** for hotel data and reservations.  

### Key Features  
- **User Experience**: personal bookings cart, favourites page, responsive design across all devices.  
- **Identity & Security**: built-in Identity UI for authentication and authorization.  
- **Admin Panel**: monitors all bookings with detailed data, filtering, ordering, and grouping options.  
- **Real-Time Updates**: SignalR with WebSocket connections for instant admin notifications.  
- **Event-Driven System**: integrated Kafka for logging and handling client operations.  
- **Email Notifications**: automated email sender using **MailKit** for new user registrations and cart checkouts.  
- **Testing & Quality**: comprehensive unit and integration tests using **xUnit** and **Moq**.  
- **Containerization**: Dockerized SQL database, Ollama client, and Kafka services.  

## 🛠 Tech Stack  

<p align="center">
  <img src="https://upload.wikimedia.org/wikipedia/commons/e/ee/.NET_Core_Logo.svg" alt="ASP.NET Core" width="60" height="60"/>
  <img src="https://upload.wikimedia.org/wikipedia/commons/4/4f/Csharp_Logo.png" alt="C#" width="60" height="60"/>
  <img src="https://github.com/user-attachments/assets/10541c91-be97-4ac5-b031-19907ff3e54f" height="32" width="32">&nbsp;
  <img src="https://www.svgrepo.com/show/303229/microsoft-sql-server-logo.svg" alt="SQL Server" width="60" height="60"/>
  <img src="https://icon.icepanel.io/Technology/svg/Apache-Kafka.svg" alt="Kafka" width="60" height="60"/>
  <img src="https://github.com/user-attachments/assets/c0672c53-68e7-4bb0-bb9f-41fadd727a4d" alt="SignalR" height="60" width="60">
  <img src="https://github.com/user-attachments/assets/a5ba288a-2e2a-4e58-bd39-0e57de4fe4da" alt="XUnit" height="60" width="60">
  <img src="https://api.nuget.org/v3-flatcontainer/moq/4.20.72/icon" alt="Moq" width="60" height="60"/>
  <img src="https://github.com/user-attachments/assets/63aa2ba4-cde5-4914-9dff-e1f36fbce961" alt="MailKit" width="60" height="60">
  <img alt="HTML" height="60" width="60" src="https://cdn.jsdelivr.net/gh/devicons/devicon@latest/icons/html5/html5-original.svg" /> 
  <img alt="CSS" height="60" width="60" src="https://cdn.jsdelivr.net/gh/devicons/devicon@latest/icons/css3/css3-original.svg" /> 
  <img alt="JavaScript" height="60" width="60" src="https://cdn.jsdelivr.net/gh/devicons/devicon@latest/icons/javascript/javascript-original.svg" /> 
</p>  

## EasyBook — Local AI Setup

<!-- This README documents how to run a local LLM via Ollama for EasyBook. 
It uses Qwen 2.5 7B Instruct as the default model. -->

### Prerequisites
- Docker Desktop installed and running
- ~5 GB free disk space
- Port `11434` available

### Quick Start

```bash
# 1) Create & start the Ollama container (persists models in a named volume "ollama")
docker run -d --name ollama --restart unless-stopped \
  -p 11434:11434 -v ollama:/root/.ollama ollama/ollama

# 2) Download the model (Qwen 2.5 7B Instruct)
docker exec -it ollama ollama pull qwen2.5:7b-instruct

# 3) (Optional) Open an interactive chat session in the container
# docker exec -it ollama ollama run qwen2.5:7b-instruct
```


## 📸 Screenshots  

### Home Page  
<img src="screenshots/home.png" alt="Home Page" width="800">  

### Booking Cart  
<img src="screenshots/cart.png" alt="Booking Cart" width="800">  

### Admin Panel  
<img src="screenshots/admin.png" alt="Admin Panel" width="800">  
