export class Item {
    title: string;
    serial: string;
    description: string;
    salePrice: number;
    id?: number;
  
    constructor(
        title: string,
        serial: string,
        description: string,
        salePrice: number,
        id?: number
    ) {
      this.id = id;
      this.title = title;
      this.serial = serial;
      this.description = description;
      this.salePrice = salePrice;
    }
  }
  export function cloneItem(i): Item {
    return new Item(
      i.title,
      i.serial,
      i.description,
      i.salePrice,
      i.id,
    );
  }
  