
import axios from 'axios';
import { getCookie, setCookie, deleteCookie, hasCookie } from 'cookies-next';

const axiosInstance = axios.create({
  baseURL: 'http://192.168.1.123:5016', 
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
