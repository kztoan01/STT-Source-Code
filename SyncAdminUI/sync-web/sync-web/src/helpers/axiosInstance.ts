
import axios from 'axios';
import { getCookie, setCookie, deleteCookie, hasCookie } from 'cookies-next';

const axiosInstance = axios.create({
  baseURL: 'http://13.211.134.159:8080', 
});

axiosInstance.interceptors.request.use(
  (config) => {
    const token = getCookie("token")
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  },
  (error) => {
    return Promise.reject(error);
  }
);

export default axiosInstance;
