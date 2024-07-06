<script>
  import { Item } from "../data/Item";
  import {
    faEdit,
    faPlus,
    faSpinner,
    faRefresh,
    faTrash,
    faCheck,
  } from "@fortawesome/free-solid-svg-icons";
  import Fa from "svelte-fa";
  import {
    Button,
    Dialog,
    DialogResult,
    DialogUtils,
    NumberField,
    TextField,
  } from "ui-commons";
  import { sl } from "../di";
  import { tItemService } from "../services/ItemService";

  export let ItemID = null;
  export let ItemSerial = "";
  export let ItemTitle = "";
  export let ItemSelected = (id, serial, title) => {};

  let itemService = sl.resolve(tItemService);

  let itemList = [];
  let isLoading = true;
  function refreshList() {
    isLoading = true;
    itemService.getItems(0, 100).then((res) => {
      itemList = res;
      isLoading = false;
    });
  }
  refreshList();

  function onItemSelected(ven) {
    ItemID = ven.id;
    ItemSerial = ven.serial;
    ItemTitle = ven.title;
    ItemSelected(ItemID, ItemSerial, ItemTitle);
  }
</script>

<table class="w-full text-sm text-left rtl:text-right text-gray-500">
  <thead class="text-xs text-gray-700 uppercase bg-gray-50">
    <tr>
      <th scope="col" class="px-6 py-3"> Serial</th>
      <th scope="col" class="px-6 py-3"> Title</th>
      <th scope="col" class="px-6 py-3"> Sales Price</th>
      <th scope="col" class="px-6 py-3"> Description</th>
      <th scope="col" class="px-6 py-3"> Operation </th>
    </tr>
  </thead>
  <tbody>
    {#each itemList as ven}
      <!-- content here -->

      <tr class="bg-white border-b">
        <th
          scope="row"
          class="px-6 py-4 font-medium text-gray-900 whitespace-nowrap"
        >
          {ven.serial}
        </th>
        <td class="px-6 py-4"> {ven.title} </td>
        <td class="px-6 py-4"> {ven.salePrice} </td>
        <td class="px-6 py-4"> {ven.description} </td>
        <td class="px-6 py-4">
          <div class="flex gap-1 ops">
            <div on:click={() => onItemSelected(ven)}>
              <Fa
                icon={faCheck}
                class="hover:scale-125 transition-all cursor-pointer"
              />
            </div>
          </div>
        </td>
      </tr>{/each}
  </tbody>
</table>
