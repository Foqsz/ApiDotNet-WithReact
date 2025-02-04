import axios from "axios";

const api = axios.create
({
  baseURL: "https://localhost:7266",
});

export default api;
