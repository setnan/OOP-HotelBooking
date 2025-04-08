// const API_BASE_URL = 'https://hotelbooking-api-69s2.onrender.com';
export const API_BASE_URL = "http://localhost:5127";

class ApiService {
  constructor() {
    this.baseUrl = API_BASE_URL;
  }

  getHeaders() {
    const token = localStorage.getItem("token");
    return {
      "Content-Type": "application/json",
      ...(token && { Authorization: `Bearer ${token}` }),
    };
  }

  async get(endpoint) {
    const response = await fetch(`${this.baseUrl}${endpoint}`, {
      method: "GET",
      headers: this.getHeaders(),
    });
    if (!response.ok) {
      const error = await response.text();
      throw new Error(error || `API Error: ${response.status}`);
    }
    return response.json();
  }

  async post(endpoint, data) {
    const response = await fetch(`${this.baseUrl}${endpoint}`, {
      method: "POST",
      headers: this.getHeaders(),
      body: JSON.stringify(data),
    });
    if (!response.ok) {
      const error = await response.text();
      throw new Error(error || `API Error: ${response.status}`);
    }
    return response.json();
  }

  async put(endpoint, data) {
    const response = await fetch(`${this.baseUrl}${endpoint}`, {
      method: "PUT",
      headers: this.getHeaders(),
      body: JSON.stringify(data),
    });
    if (!response.ok) {
      const error = await response.text();
      throw new Error(error || `API Error: ${response.status}`);
    }
    return response.json();
  }

  async delete(endpoint) {
    const response = await fetch(`${this.baseUrl}${endpoint}`, {
      method: "DELETE",
      headers: this.getHeaders(),
    });
    if (!response.ok) {
      const error = await response.text();
      throw new Error(error || `API Error: ${response.status}`);
    }
    return response.json();
  }

  // Authentication endpoints
  async login(credentials) {
    return this.post("/auth/login", credentials);
  }

  // User endpoints
  async getUsers() {
    return this.get("/users");
  }

  async createUser(userData) {
    return this.post("/users", userData);
  }

  // Room endpoints
  async getRooms() {
    return this.get("/rooms");
  }

  async getRoom(roomId) {
    return this.get(`/rooms/${roomId}`);
  }

  // Booking endpoints
  async getBookings() {
    return this.get("/bookings");
  }

  async createBooking(bookingData) {
    return this.post("/bookings", bookingData);
  }

  async updateBooking(bookingId, bookingData) {
    return this.put(`/bookings/${bookingId}`, bookingData);
  }

  async deleteBooking(bookingId) {
    return this.delete(`/bookings/${bookingId}`);
  }
}

export const api = new ApiService();
