import type { User } from "../data/User";
import { HttpModule, tHttpModule } from "./http";
import { Inject, Injectable } from "container-ioc";

export const tStoreService = Symbol("storeService");
@Injectable()
export class StoreService {
  constructor(@Inject(tHttpModule) private http: HttpModule) {}
  async initializeStore({ title, address }) {
    const url = this.http.url("general/initialize");
    return (await this.http.post(url, { title, address, initializationDate: new Date()})).data;
  }

  async isStoreInitialized(): Promise<boolean> {
    const url = this.http.url("general/isStoreInitialized");

    return (await this.http.get(url)).data as boolean;
  }

  async getStore(){
    const url = this.http.url("general/store")

    return (await this.http.get(url)).data;
  }
}
