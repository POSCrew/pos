export class Item {
    title: string;
    serial: string;
    description: string;
    salePriceStr: string; // TODO: change to number after adding number field !!!
    salePrice: number; // TODO: change to number after adding number field !!!
    id?: number;
  
    constructor(
        title: string,
        serial: string,
        description: string,
        salePrice: number, // TODO: change to number after adding number field !!!
        id?: number
    ) {
      this.id = id;
      this.title = title;
      this.serial = serial;
      this.description = description;
      this.salePriceStr = salePrice.toString();
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
  