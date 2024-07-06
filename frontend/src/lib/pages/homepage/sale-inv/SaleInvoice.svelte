<script lang="ts">
  import { DatePicker, NumberField, TextField } from "ui-commons";
  import MainArea from "./MainArea.svelte";
  import NavigationMenu from "./NavigationMenu.svelte";
  import {
    tVendorService,
    VendorService,
  } from "../../../services/vendorService";
  import { sl } from "../../../di";
  import {
    CustomerService,
    tCustomerService,
  } from "../../../services/CustomerService";
  import { ItemService, tItemService } from "../../../services/ItemService";
  import { Item } from "../../../data/Item";
  import { Vendor } from "../../../data/Vendor";
  import { Customer } from "../../../data/Customer";

  let vendorService: VendorService = sl.resolve(tVendorService);
  let customerService: CustomerService = sl.resolve(tCustomerService);
  let itemService: ItemService = sl.resolve(tItemService);
  let items: Item[] = $state([]);
  let customers: Customer[] = $state([]);
  itemService.getItems().then((response) => {
    items = response;
  });
  customerService.getCustomers().then((res) => {
    customers = res;
  });

  let customer: Customer = $state(null);
  let lastItem: Item = $state(null);
</script>

<main class="flex min-h-full">
  <!-- content here -->
  <div class="flex-grow-0 w-96">
    <NavigationMenu
      {items}
      {customers}
      on:onCustomerSelected={(c) => {
        customer = c.detail;
      }}
      on:onItemAdd={(e) => {
        lastItem = e.detail;
        setTimeout(() => {
          lastItem = null;
        });
      }}
    />
  </div>
  <div class="grow w-0">
    <MainArea {customer} item={lastItem} />
  </div>
</main>
