<script lang="ts">
  import Fa from "svelte-fa";
  import type { Customer } from "../../../data/Customer";
  import {
    faChevronDown,
    faChevronUp,
    faTrash,
  } from "@fortawesome/free-solid-svg-icons";
  import type { Item } from "../../../data/Item";
  import { untrack } from "svelte";
  import { flip } from "svelte/animate";

  let { customer, item } = $props<{ customer: Customer; item: Item }>();
  let invoiceItems = $state([]);

  $effect(() => {
    if (item !== null) {
      untrack(() => {
        let currentItem = invoiceItems.find((e) => {
          return e.itemId === item.id;
        });
        if (!currentItem) {
          invoiceItems.push({
            rowNumber: invoiceItems.length,
            itemId: item.id,
            itemTitle: item.title,
            itemSerial: item.serial,
            quantity: 1,
            price: item.salePrice,
          });
        } else {
          currentItem.quantity++;
        }
      });
    }
  });

  function deleteItem(ind) {
    invoiceItems.splice(ind, 1);
  }

  function moveUp(i) {
    if (i === 0) return;
    let temp;
    temp = invoiceItems[i - 1];
    invoiceItems[i - 1] = invoiceItems[i];
    invoiceItems[i] = temp;
  }
  function moveDown(i) {
    if (i === invoiceItems.length - 1) return;
    let temp;
    temp = invoiceItems[i + 1];
    invoiceItems[i + 1] = invoiceItems[i];
    invoiceItems[i] = temp;
  }
</script>

<table class="w-full text-sm text-left rtl:text-right text-gray-500">
  <thead class="text-xs text-gray-700 uppercase bg-gray-50">
    <tr>
      <th scope="col" class="px-6 py-3"> Row number</th>
      <th scope="col" class="px-6 py-3"> Item serial </th>
      <th scope="col" class="px-6 py-3"> Item title</th>
      <th scope="col" class="px-6 py-3"> Quantity </th>
      <th scope="col" class="px-6 py-3"> Price </th>
      <th scope="col" class="px-6 py-3"> Fee </th>
      <th scope="col" class="px-6 py-3"> Delete </th>
    </tr>
  </thead>

  <tbody>
    {#each invoiceItems as invItem, i (invItem.itemId)}
      <tr class="bg-white border-b" animate:flip={{}}>
        <th
          scope="row"
          class="px-6 py-4 font-medium text-gray-900 whitespace-nowrap flex justify-center"
        >
          <div class="mr-1">
            <div on:click={() => moveUp(i)}>
              <Fa icon={faChevronUp} />
            </div>
            <div on:click={() => moveDown(i)}>
              <Fa icon={faChevronDown} />
            </div>
          </div>
        </th>
        <td class="px-6 py-4"> {invItem.itemSerial} </td>
        <td class="px-6 py-4"> {invItem.itemTitle} </td>
        <td class="px-6 py-4"> {invItem.quantity} </td>
        <td class="px-6 py-4"> {invItem.price} </td>
        <td class="px-6 py-4"> {invItem.price * invItem.quantity} </td>
        <td class="px-6 py-4">
          <div class="flex gap-1 ops">
            <div on:click={() => deleteItem(i)}>
              <Fa
                icon={faTrash}
                color="red"
                class="hover:scale-125 transition-all cursor-pointer"
              />
            </div>
          </div>
        </td>
      </tr>{/each}
  </tbody>
</table>
