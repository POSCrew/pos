<script lang="ts">
  import {
    faArrowLeft,
    faArrowRight,
    faEdit,
    faEye,
    faPlus,
    faRefresh,
    faSpinner,
    faTrash,
  } from "@fortawesome/free-solid-svg-icons";
  import {
    Button,
    DatePicker,
    Dialog,
    DialogResult,
    DialogUtils,
    NumberField,
    TextField,
  } from "ui-commons";
  import Fa from "svelte-fa";
  import { cloneVendor, Vendor } from "../../../data/Vendor";
  import { sl } from "../../../di";
  import {
    tVendorService,
    VendorService,
  } from "../../../services/vendorService";
  import type { AxiosError } from "axios";
  import { dialogErrorHandler } from "../../../utils/svelte-utils";
  import {
    tPurchaseInvoiceService,
    type PurchaseInvoiceService,
  } from "../../../services/PurchaseInvoiceService";
  import { navigate } from "svelte-navigator";
  let purchaseInvoiceService: PurchaseInvoiceService = sl.resolve(
    tPurchaseInvoiceService,
  );

  let newDialogOpen = $state(false);

  let pageSize = 10;
  let currentPage: number = $state(1);
  let totalPages: number = $state(1);

  let purchaseInvoiceList: any[] = $state([]);
  let isLoading = $state(true);
  function refreshList() {
    isLoading = true;

    purchaseInvoiceService.getCount().then((res) => {
      totalPages = Math.ceil(res / pageSize);
      if (currentPage > totalPages) currentPage = 0;
    });

    purchaseInvoiceService.getItems(currentPage - 1, pageSize).then((res) => {
      purchaseInvoiceList = res;
      isLoading = false;
    });
  }
  refreshList();

  function decreasePage() {
    if (currentPage > 1) currentPage -= 1;
    refreshList();
  }

  function increasePage() {
    if (currentPage < totalPages) currentPage += 1;
    refreshList();
  }

  function onNewPurchaseInvoice() {
    navigate("../purchase-inv");
  }

  function onCancel() {}

  function deleteItem(purchaseInvoice) {
    DialogUtils.confirmation(
      `Do you really want to delete this invoice with number : ${purchaseInvoice.number || ""}?\n`,
    ).then((res) => {
      console.log("result : ", res);

      if (res === DialogResult.OK) {
        purchaseInvoiceService.remove(purchaseInvoice.id).then(() => {
          refreshList();
        });
      }
    });
  }

  let isViewDialogOpen = $state(false);
  let viewedItem = $state(null);
  async function viewItem(id) {
    viewedItem = await purchaseInvoiceService.getItem(id);
    isViewDialogOpen = true;
  }
</script>

<div class="relative overflow-x-auto p-5">
  <div class="ml-2 mb-2 inline-flex">
    <Button
      hoverColor="#6ff1c4bb"
      borderColor="#57bf9a"
      color="#6ff1c4"
      borderThickness="1"
      on:click={onNewPurchaseInvoice}
    >
      <Fa icon={faPlus} /> New
    </Button>
    <div class="m-1" />
    <Button
      hoverColor="#6ff1c4bb"
      borderColor="#57bf9a"
      color="#6ff1c4"
      borderThickness="1"
      on:click={refreshList}
    >
      <Fa icon={faRefresh} />
    </Button>
  </div>

  <table class="w-full text-sm text-left rtl:text-right text-gray-500">
    <thead class="text-xs text-gray-700 uppercase bg-gray-50">
      <tr>
        <th scope="col" class="px-6 py-3"> Number </th>
        <th scope="col" class="px-6 py-3"> Descrioption </th>
        <th scope="col" class="px-6 py-3"> Date </th>
        <th scope="col" class="px-6 py-3"> Customer Name </th>
        <th scope="col" class="px-6 py-3"> Total price</th>

        <th scope="col" class="px-6 py-3"> Operation </th>
      </tr>
    </thead>
    <tbody>
      {#each purchaseInvoiceList as si}
        <!-- content here -->

        <tr class="bg-white border-b">
          <th
            scope="row"
            class="px-6 py-4 font-medium text-gray-900 whitespace-nowrap"
          >
            {si.number}
          </th>
          <td class="px-6 py-4"> {si.description} </td>
          <td class="px-6 py-4"> {si.date} </td>
          <td class="px-6 py-4">
            {si.customer?.firstName || ""}
            {si.customer?.lastName || ""}
          </td>
          <td class="px-6 py-4"> {si.totalPrice} </td>
          <td class="px-6 py-4">
            <div class="flex gap-1 ops">
              <div on:click={() => deleteItem(si)}>
                <Fa
                  icon={faTrash}
                  color="red"
                  class="hover:scale-125 transition-all cursor-pointer"
                />
              </div>
              <div on:click={() => viewItem(si.id)}>
                <Fa
                  icon={faEye}
                  class="hover:scale-125 transition-all cursor-pointer"
                />
              </div>
            </div>
          </td>
        </tr>{/each}
    </tbody>
  </table>
  {#if isLoading}
    <div class="flex justify-center items-center mt-4">
      <Fa icon={faSpinner} size="3x" spin />
    </div>
  {:else}
    <div class="flex justify-center items-center mt-2">
      <Button
        hoverColor="#bfbfbfbb"
        borderColor="#bfbfbf"
        color="#bfbfbf"
        borderThickness="1"
        on:click={decreasePage}
      >
        <Fa icon={faArrowLeft} />
      </Button>
      <div class="m-1" />
      <div>
        page {currentPage} of {totalPages}
      </div>
      <div class="m-1" />
      <Button
        hoverColor="#bfbfbfbb"
        borderColor="#bfbfbf"
        color="#bfbfbf"
        borderThickness="1"
        on:click={increasePage}
      >
        <Fa icon={faArrowRight} />
      </Button>
    </div>
  {/if}
</div>

<Dialog bind:isOpen={isViewDialogOpen}>
  {#if viewedItem}
    <!-- content here -->

    <div class="flex flex-col h-[550px] overflow-y-auto">
      <div class="grow flex flex-col">
        <div
          class="m-8 p-4 border-[1.5px] rounded-sm border-gray-400 flex-grow-0"
        >
          <h3 class="outline-none">
            Customer Name: {#if viewedItem.customer}
              <!-- content here -->

              {viewedItem.customer?.code || "(...)"} - {viewedItem.customer
                ? viewedItem.customer.firstName ||
                  "" + viewedItem.customer.lastName ||
                  ""
                : "(...)"}
            {/if}
          </h3>
          <div class="flex gap-2">
            <DatePicker
              bind:value={viewedItem.date}
              label="Invoive Date : "
              disabled
            />
            <NumberField
              bind:value={viewedItem.number}
              label="Invoice number : "
              disabled={true}
            />
          </div>
          <TextField
            label="Description"
            bind:value={viewedItem.description}
            disabled={true}
          />
        </div>

        <div class="h-0 grow overflow-y-auto">
          <table class="text-sm text-left rtl:text-right text-gray-500 w-full">
            <thead class="text-xs text-gray-700 uppercase bg-gray-50">
              <tr>
                <th scope="col" class="px-6 py-3"> Row number</th>
                <th scope="col" class="px-6 py-3"> Item serial </th>
                <th scope="col" class="px-6 py-3"> Item title</th>
                <th scope="col" class="px-6 py-3"> Quantity </th>
                <th scope="col" class="px-6 py-3"> Fee </th>
                <th scope="col" class="px-6 py-3"> Price </th>
              </tr>
            </thead>

            <tbody>
              {#each viewedItem.invoiceItems as invItem, i}
                <tr class="bg-white border-b">
                  <th
                    scope="row"
                    class="px-6 py-4 font-medium text-gray-900 whitespace-nowrap flex justify-center"
                  >
                    <div class="mr-1">
                      <!-- <div on:click={() => moveUp(i)}>
                      <Fa icon={faChevronUp} />
                    </div>
                    <div on:click={() => moveDown(i)}>
                      <Fa icon={faChevronDown} />
                    </div> -->
                    </div>
                  </th>
                  <td class="px-6 py-4"> {invItem.item.serial} </td>
                  <td class="px-6 py-4"> {invItem.item.title} </td>
                  <td class="px-6 py-4">
                    <NumberField value={invItem.quantity} disabled={true} />
                  </td>
                  <td class="px-6 py-4">
                    <NumberField value={invItem.price} disabled={true} />
                  </td>
                  <td class="px-6 py-4">
                    {invItem.price * invItem.quantity}
                  </td>
                </tr>{/each}
            </tbody>
          </table>
        </div>
      </div>

      <div class="flex-grow-0 border-t-2 p-2 px-6 flex items-center">
        <div class="mr-auto"></div>
        <p class="mr-6">
          Total price:
          {viewedItem.totalPrice}
        </p>
      </div>
    </div>
  {/if}
</Dialog>

<style lang="postcss">
  .ops {
    svg {
      @apply hover:scale-125 transition-all cursor-pointer;
    }
  }
</style>
