import axios from "axios";
export const serverUrl = import.meta.env.VITE_BACKEND_URL;

export class Http {
  private static instance: Http = new Http();

  static get() {
    if (this.instance === null) {
      this.instance = new Http();
    }
    return this.instance;
  }

  post(url, body) {
    return axios.post(serverUrl + url, body);
  }
}
