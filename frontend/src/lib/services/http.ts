import axios from "axios";
import { Injectable } from "container-ioc";
import { API_ROOT_URL, Constants } from "../config/config";
import { dialogErrorHandler } from "../utils/svelte-utils";

axios.interceptors.request.use(
  (config) => {
    const { origin } = new URL(config.url);
    const allowedOrigins = [API_ROOT_URL];
    if (allowedOrigins.includes(origin)) {
      config.withCredentials = true;
    }
    return config;
  },
  (error) => {
    return Promise.reject(error);
  },
);
axios.interceptors.response.use(undefined, (error) => {
  dialogErrorHandler(error);
  return Promise.reject(error);
});

export const tHttpModule = Symbol("http");
@Injectable()
export class HttpModule {
  get = axios.get;
  patch = axios.patch;
  put = axios.put;
  post = axios.post;
  delet = axios.delete;
  isStoredAccessToken(): boolean {
    return Boolean(localStorage.getItem(Constants.accessTokenKey));
  }

  url(path: string) {
    return API_ROOT_URL + "/" + path;
  }
}

export const get = axios.get;
export const patch = axios.patch;
export const post = axios.post;
export const delet = axios.delete;
