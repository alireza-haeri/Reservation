# 🚀 Cinema Reservation System with RedLock

> A simple .NET demo project to illustrate how to prevent race conditions in a distributed environment using **RedLock.net** and **Redis**.

[![Status](https://img.shields.io/badge/Status-Demo-green.svg)](https://github.com/alireza-haeri/Reservation)
[![Technology](https://img.shields.io/badge/Tech-.NET%20%7C%20Redis-blueviolet.svg)](https://github.com/alireza-haeri/Reservation)
[![License](https://img.shields.io/badge/License-MIT-blue.svg)](/LICENSE)

<br>

## 🎯 Project Goal

The purpose of this project is not to build a full-featured reservation system, but to provide a clear and simple example of solving a common concurrency problem: **race conditions**.

It demonstrates how to implement a **distributed lock** to ensure that a critical resource (like a cinema seat) can only be processed by one request at a time, even when multiple servers or instances are running.

---

## ❓ The Problem: Race Condition

Imagine a popular movie has only **one seat left**. Two different users click the "Book Now" button at the exact same moment.

*   Request A reads the database and sees "1 seat available".
*   Request B reads the database at the same time and also sees "1 seat available".
*   Request A processes the booking and updates the seat count to 0.
*   Request B, unaware of Request A's action, also processes the booking and updates the seat count to 0.

**Result:** The same seat is sold twice, leading to data inconsistency and an unhappy customer.

## ✅ The Solution: Distributed Lock with RedLock

This project uses `RedLock.net`, a C# implementation of the RedLock algorithm, to solve this problem.

Before attempting to book a seat, the application tries to acquire a **lock** on that specific seat's ID.

1.  A request comes in to book `Seat-7`.
2.  The system asks Redis (via RedLock) to create a lock for the resource key `"seat-7"`.
3.  **If the lock is acquired successfully:** The application proceeds with the booking logic. No other request can get a lock for `"seat-7"` until this one is finished and releases the lock.
4.  **If another request for `Seat-7` arrives:** It will fail to acquire the lock and will be informed that the resource is currently busy (locked), preventing the double-booking.

This ensures that the process of "checking availability and booking" is **atomic** across a distributed system.

---

## 🛠️ Core Technologies

*   **C#** & **ASP.NET Core:** For building the web API.
*   **Redis:** As the distributed cache and lock manager.
*   **RedLock.net:** The client library used to implement the distributed lock algorithm.

---

## 🚀 How to Run and Test

### Prerequisites

*   [.NET SDK (6.0 or later)](https://dotnet.microsoft.com/download)
*   [Docker](https://www.docker.com/products/docker-desktop/) (or a running Redis instance)

### Steps

1.  **Start a Redis instance:**
    The easiest way is using Docker:
    ```sh
    docker run --name my-redis -p 6379:6379 -d redis
    ```

2.  **Clone the repository:**
    ```sh
    git clone https://github.com/alireza-haeri/Reservation.git
    cd Reservation
    ```

3.  **Configure the application:**
    Ensure the `appsettings.json` file points to your Redis instance (the default `localhost:6379` should work with the Docker command above).

4.  **Run the project:**
    ```sh
    dotnet run
    ```

### Testing the Lock

To see the lock in action, use an API client like Postman or `curl` to send multiple, near-simultaneous requests to the booking endpoint.

*   **Endpoint:** `POST /api/reservation`
*   **Body:**
    ```json
    {
      "seatId": "A1"
    }
    ```

Send 5-10 requests for the **same `seatId`** at once. You will observe that only **one** request succeeds (e.g., returns `200 OK`), while the others will fail (e.g., return `409 Conflict` or a similar status), proving that the distributed lock correctly prevented the race condition.

---

## 📫 Contact

*   **Email:** alireza.haeri.dev@gmail.com
*   **Telegram:** [@AlirezaHaeriDev](https://t.me/AlirezaHaeriDev)
*   **LinkedIn:** [in/alireza-haeri-dev](https://www.linkedin.com/in/alireza-haeri-dev)
