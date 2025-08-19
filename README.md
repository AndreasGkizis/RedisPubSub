### Redis Pub/Sub Example

This repository contains a simple .NET Core application demonstrating the **publish/subscribe pattern** using **Redis**. 
The solution includes multiple services—publishers and consumers—that communicate through a shared Redis instance. 

---

### Prerequisites

Before you begin, ensure you have the following installed on your system:

-   [.NET SDK](https://dotnet.microsoft.com/download) (Version 9 or higher is recommended)
-   `Docker` or `Podman` or whatever else you might be using instead configured. The examples will be using `podman`

---

### Architecture

The application uses **Docker Compose** to manage three types of services:

1.  **Redis Server**: The central message broker.
2.  **Publisher Services**: Continuously send messages to a specific Redis channel.
3.  **Consumer Services**: Subscribe to the same Redis channel and print any incoming messages to the console.

All services are connected via a shared network.

---

### Getting Started

Follow these steps to set up and run the application.

#### 1. Build the Docker Image

Navigate to the project's root directory (the same directory as your `docker-compose.yml` file) and build the Docker image for your backend application.

```bash
podman compose up -d --build
```
This command will:

- Start a Redis container.

- Start two publisher containers (publisher-service-1 and 2).

- Start two consumer containers (consumer-service-1 and 2).

You should see logs in the containers publishers sending messages and 
consumers receiving messages **FROM BOTH PUBLISHERS**.

3. Stopping the Services
To stop all the running containers and remove the network, press Ctrl+C in your terminal and then run:
```bash
 podman compose down
```

to also remove the images created run 

```bash
 podman image prune
```

This should gracefully shut down and remove all the services.