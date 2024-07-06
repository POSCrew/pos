import { HttpModule, tHttpModule } from "./http";
import { Inject, Injectable } from "container-ioc";

export const tSaleInvoiceService = Symbol("SaleInvoiceService");
@Injectable()
export class SaleInvoiceService {
  constructor(@Inject(tHttpModule) private http: HttpModule) {}
  async create(item){
    const url = this.http.url("sales/saleInvoices");

    return (await this.http.post(url, item)).data ;
  }

  async getItem(id: number){
    const url = this.http.url("sales/saleInvoices");

    return (await this.http.get(url, {params: {id}})).data ;
  }

  async getItems(page:number=null, pageSize:number=null){
    const url = this.http.url("sales/saleInvoices/all")

    return (await this.http.get(url, {params:{page, pageSize}})).data as [];
  }

  async getCount(){
    const url = this.http.url("sales/saleInvoices/count")

    return (await this.http.get(url)).data as number;
  }

  async remove(id: number){
    const url = this.http.url("sales/saleInvoices");

    await this.http.delet(url, {params: {id}});
  }

  async update(item){
    const url = this.http.url("sales/saleInvoices");

    return (await this.http.put(url, item)).data;
  }
}
