export class Customer {
    id?: number;
    code: string;
    firstName: string;
    lastName: string;
    phoneNumber: string;
    address: string;
  
    constructor(
      code: string,
      firstName: string,
      lastName: string,
      phoneNumber: string,
      address: string,
      id?: number,
    ) {
      this.id = id;
      this.code = code;
      this.firstName = firstName;
      this.lastName = lastName;
      this.phoneNumber = phoneNumber;
      this.address = address;
    }
  }
  export function cloneCustomer(c): Customer {
    return new Customer(
      c.code,
      c.firstName,
      c.lastName,
      c.phoneNumber,
      c.address,
      c.id,
    );
  }
  