import type { Pricing } from "../data/Pricing";
import { HttpModule, tHttpModule } from "./http";
import { Inject, Injectable } from "container-ioc";

export const tPricingService = Symbol("pricingService");
@Injectable()
export class PricingService {
  constructor(@Inject(tHttpModule) private http: HttpModule) {}
  async create(pricing: Pricing){
    const url = this.http.url("sales/pricings");

    await this.http.post(url, pricing);
  }

  async getNewPricingStartDate(){
    const url = this.http.url("sales/pricings/newPricingStartDate")

    return (await this.http.get(url)).data as Date;
  }

  async getPricings(page:number, pageSize:number){
    const url = this.http.url("sales/pricings/all")

    return (await this.http.get(url, {params:{page, pageSize}})).data as Pricing[];
  }

  async getCount(){
    const url = this.http.url("sales/pricings/count")

    return (await this.http.get(url)).data as number;
  }

  async removeLastPricing(){
    const url = this.http.url("sales/pricings");

    await this.http.delet(url);
  }
}
