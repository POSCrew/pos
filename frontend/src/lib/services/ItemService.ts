import type { Item } from "../data/Item";
import { HttpModule, tHttpModule } from "./http";
import { Inject, Injectable } from "container-ioc";

export const tItemService = Symbol("itemService");
@Injectable()
export class ItemService {
  constructor(@Inject(tHttpModule) private http: HttpModule) {}
  async create(item: Item){
    const url = this.http.url("inventory/items");

    return (await this.http.post(url, item)).data as Item;
  }

  async getItem(id: number){
    const url = this.http.url("inventory/items");

    return (await this.http.get(url, {params: {id}})).data as Item;
  }

  async getItems(page:number=null, pageSize:number=null){
    const url = this.http.url("inventory/items/all")

    return (await this.http.get(url, {params:{page, pageSize}})).data as Item[];
  }

  async getCount(){
    const url = this.http.url("inventory/items/count")

    return (await this.http.get(url)).data as number;
  }

  async remove(id: number){
    const url = this.http.url("inventory/items");

    await this.http.delet(url, {params: {id}});
  }

  async update(item: Item){
    const url = this.http.url("inventory/items");

    return (await this.http.put(url, item)).data as Item;
  }
}
