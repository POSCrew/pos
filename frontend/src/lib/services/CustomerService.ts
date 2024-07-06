import type { Customer } from "../data/Customer";
import { HttpModule, tHttpModule } from "./http";
import { Inject, Injectable } from "container-ioc";

export const tCustomerService = Symbol("customerService");
@Injectable()
export class CustomerService {
  constructor(@Inject(tHttpModule) private http: HttpModule) {}
  async create(customer: Customer){
    const url = this.http.url("sales/customers");

    return (await this.http.post(url, customer)).data as Customer;
  }

  async getCustomer(id: number){
    const url = this.http.url("sales/customers");

    return (await this.http.get(url, {params: {id}})).data as Customer;
  }

  async getCustomers(page:number=null, pageSize:number=null){
    const url = this.http.url("sales/customers/all")

    return (await this.http.get(url, {params:{page, pageSize}})).data as Customer[];
  }

  async getCount(){
    const url = this.http.url("sales/customers/count")

    return (await this.http.get(url)).data as number;
  }

  async remove(id: number){
    const url = this.http.url("sales/customers");

    await this.http.delet(url, {params: {id}});
  }

  async update(customer: Customer){
    const url = this.http.url("sales/customers");

    return (await this.http.put(url, customer)).data as Customer;
  }
}
