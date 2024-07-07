<script lang="ts">
  import Fa from "svelte-fa";
  import type { Vendor } from "../../../data/Vendor";
  import {
    faChevronDown,
    faChevronUp,
    faList,
    faSave,
    faTrash,
  } from "@fortawesome/free-solid-svg-icons";
  import type { Item } from "../../../data/Item";
  import { untrack } from "svelte";
  import { flip } from "svelte/animate";
  import {
    Button,
    DatePicker,
    DialogUtils,
    NumberField,
    TextField,
  } from "ui-commons";
  import { isoDate } from "../../../utils/dateUtils";
  import { sl } from "../../../di";
  import {
    PurchaseInvoiceService,
    tPurchaseInvoiceService,
  } from "../../../services/PurchaseInvoiceService";
  import { navigate } from "svelte-navigator";

  const invoiceService: PurchaseInvoiceService = sl.resolve(
    tPurchaseInvoiceService,
  );

  let { vendor, item } = $props<{ vendor: Vendor; item: Item }>();
  let invoiceItems = $state([]);

  $effect(() => {
    if (item !== null) {
      untrack(() => {
        let currentItem = invoiceItems.find((e) => {
          return e.itemId === item.id;
        });
        if (!currentItem) {
          invoiceItems.push({
            rowNumber: invoiceItems.length + 1,
            itemId: item.id,
            itemTitle: item.title,
            itemSerial: item.serial,
            quantity: 1,
            price: 0,
          });
        } else {
          currentItem.quantity++;
        }
      });
    }
  });

  function deleteItem(ind) {
    invoiceItems.splice(ind, 1);
    for (let i = ind; i < invoiceItems.length; i++) {
      invoiceItems[i].rowNumber--;
    }
  }

  function moveUp(i) {
    if (i === 0) return;
    invoiceItems[i].rowNumber--;
    invoiceItems[i - 1].rowNumber++;
    let temp;
    temp = invoiceItems[i - 1];
    invoiceItems[i - 1] = invoiceItems[i];
    invoiceItems[i] = temp;
  }
  function moveDown(i) {
    if (i === invoiceItems.length - 1) return;
    invoiceItems[i].rowNumber++;
    invoiceItems[i + 1].rowNumber--;
    let temp;
    temp = invoiceItems[i + 1];
    invoiceItems[i + 1] = invoiceItems[i];
    invoiceItems[i] = temp;
  }
  let invoiceDate = $state(isoDate(new Date().toISOString()));
  let invoiceNumber: number = $state();
  let description = $state("");

  function save() {
    if (!vendor) {
      DialogUtils.error("", "Vendor should be selected!");
    }
    invoiceService
      .create({
        number: invoiceNumber,
        date: new Date(invoiceDate),
        description,
        vendorId: vendor.id,
        invoiceItems,
      })
      .then((res) => {
        console.log(res);
        invoiceDate = isoDate(new Date().toISOString())
        invoiceNumber = null;
        invoiceItems = []
        vendor = null;
        description = ""
      });
  }
</script>

<div class="flex flex-col h-full overflow-y-auto">
  <div class="grow flex flex-col">
    <div class="m-8 p-4 border-[1.5px] rounded-sm border-gray-400 flex-grow-0">
      <h3 class="outline-none">
        Vendor Name: {#if vendor}
          <!-- content here -->

          {vendor?.code || "(...)"} - {vendor
            ? vendor.firstName || "" + vendor.lastName || ""
            : "(...)"}
        {/if}
      </h3>
      <div class="flex gap-2">
        <DatePicker bind:value={invoiceDate} label="Invoive Date : " />
        <NumberField bind:value={invoiceNumber} label="Invoice number : " />
      </div>
      <TextField label="Description" bind:value={description} />
    </div>

    <div class="h-0 grow overflow-y-auto">
      <table class="text-sm text-left rtl:text-right text-gray-500">
        <thead class="text-xs text-gray-700 uppercase bg-gray-50">
          <tr>
            <th scope="col" class="px-6 py-3"> Row number</th>
            <th scope="col" class="px-6 py-3"> Item serial </th>
            <th scope="col" class="px-6 py-3"> Item title</th>
            <th scope="col" class="px-6 py-3"> Quantity </th>
            <th scope="col" class="px-6 py-3"> Fee </th>
            <th scope="col" class="px-6 py-3"> Price </th>
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
              <td class="px-6 py-4">
                <NumberField bind:value={invItem.quantity} />
              </td>
              <td class="px-6 py-4">
                <NumberField bind:value={invItem.price} />
              </td>
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
    </div>
  </div>

  <div class="flex-grow-0 border-t-2 p-2 px-6 flex items-center">
    <Button
      color="#eee"
      on:click={() => {
        navigate("../purchase-inv-list");
      }}
    >
      <Fa icon={faList} />
      List
    </Button>
    <div class="mr-auto"></div>
    <p class="mr-6">
      Total price:
      {invoiceItems?.length &&
        invoiceItems
          .map((e) => e.price * e.quantity)
          .reduce((sum, e) => {
            return sum + e;
          })}
    </p>
    <Button color="#a8fe95" on:click={save}>
      <Fa icon={faSave} /> Save
    </Button>
  </div>
</div>

<style>
  button {
    color: #a8fe95;
  }
</style>
