export class Vendor {
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
export function cloneVendor(ven): Vendor {
  return new Vendor(
    ven.code,
    ven.firstName,
    ven.lastName,
    ven.phoneNumber,
    ven.address,
    ven.id,
  );
}
