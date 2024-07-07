<script lang="ts">
  import { DatePicker, NumberField, TextField } from "ui-commons";
  import MainArea from "./MainArea.svelte";
  import NavigationMenu from "./NavigationMenu.svelte";
  import {
    tVendorService,
    VendorService,
  } from "../../../services/vendorService";
  import { sl } from "../../../di";
  import { ItemService, tItemService } from "../../../services/ItemService";
  import { Item } from "../../../data/Item";
  import { Vendor } from "../../../data/Vendor";

  let vendorService: VendorService = sl.resolve(tVendorService);
  let itemService: ItemService = sl.resolve(tItemService);
  let items: Item[] = $state([]);
  let vendors: Vendor[] = $state([]);
  itemService.getItems().then((response) => {
    items = response;
  });
  vendorService.getVendors().then((res) => {
    vendors = res;
  });

  let vendor: Vendor = $state(null);
  let lastItem: Item = $state(null);
</script>

<main class="flex min-h-full">
  <!-- content here -->
  <div class="flex-grow-0 w-96">
    <NavigationMenu
      {items}
      {vendors}
      on:onVendorSelected={(c) => {
        vendor = c.detail;
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
    <MainArea {vendor} item={lastItem} />
  </div>
</main>
