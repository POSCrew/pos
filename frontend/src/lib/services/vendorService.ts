import type { Vendor } from "../data/Vendor";
import { HttpModule, tHttpModule } from "./http";
import { Inject, Injectable } from "container-ioc";

export const tVendorService = Symbol("vendorService");
@Injectable()
export class VendorService {
  constructor(@Inject(tHttpModule) private http: HttpModule) {}
  async create(vendor: Vendor){
    const url = this.http.url("inventory/vendors");

    return (await this.http.post(url, vendor)).data as Vendor;
  }

  async getVendor(id: number){
    const url = this.http.url("inventory/vendors");

    return (await this.http.get(url, {params: {id}})).data as Vendor;
  }

  async getVendors(page:number, pageSize:number){
    const url = this.http.url("inventory/vendors/all")

    return (await this.http.get(url, {params:{page, pageSize}})).data as Vendor[];
  }

  async getCount(){
    const url = this.http.url("inventory/vendors/count")

    return (await this.http.get(url)).data as number;
  }

  async remove(id: number){
    const url = this.http.url("inventory/vendors");

    await this.http.delet(url, {params: {id}});
  }

  async update(vendor: Vendor){
    const url = this.http.url("inventory/vendors");

    return (await this.http.put(url, vendor)).data as Vendor;
  }
}
