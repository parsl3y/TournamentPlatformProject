import axios from 'axios';

export class HttpClient {
  constructor(config) {
    this.axiosInstance = axios.create({
      baseURL: config.baseURL || 'http://localhost:5030',
      headers: {
        'Content-Type': 'application/json',
        'Accept': 'application/json',
        ...config.headers,
      },
      ...config,
    });

    this.initInterceptors();
  }

  async get(url, config) {
    return this.request({ method: 'GET', url, ...config });
  }

  async post(url, data, config) {
    return this.request({ method: 'POST', url, data, ...config });
  }

  async delete(url, config = {}) {
    return this.request({ method: 'DELETE', url, ...config });
  }

  async put(url, data, config = {}) {
    return this.request({ method: 'PUT', url, data, ...config });
  }

  async request(config) {
    try {
      const response = await this.axiosInstance.request(config);
      return response.data;
    } catch (error) {
      if (axios.isCancel(error)) {
        console.info('Request was cancelled');
      } else {
        console.error('Request failed with error', error.response?.statusText || error.message);
      }
      throw error; 
    }
  }

  initInterceptors() {
    this.axiosInstance.interceptors.request.use(
      (config) => {
        const apiKey = localStorage.getItem('apiKey') || 'defaultApiKey';

        if (apiKey) {
          config.params = { ...config.params, 'api-key': apiKey };
        }

        return config;
      },
      (error) => {
        console.error('Request failed with error', error);
        return Promise.reject(error);
      }
    );

    this.axiosInstance.interceptors.response.use(
      (response) => response,
      (error) => {
        if (error.response?.status === 401) {
          console.error('Unauthorized request');
          window.location.href = '/login?returnUrl=' + window.location.pathname;
        }
        return Promise.reject(error);
      }
    );
  }
}
