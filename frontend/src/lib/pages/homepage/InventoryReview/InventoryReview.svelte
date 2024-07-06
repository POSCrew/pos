<script lang="ts">
    import {
      faEdit,
      faPlus,
      faSpinner,
      faRefresh,
      faTrash,
    } from "@fortawesome/free-solid-svg-icons";
    import {
      Button,
      Dialog,
      DialogResult,
      DialogUtils,
      TextField,
    } from "ui-commons";
    import Fa from "svelte-fa";
    import { sl } from "../../../di";
  import { tInventoryReviewService, type InventoryReviewService } from "../../../services/InventoryReviewService";
  import type { InventoryReviewItems } from "../../../data/InventoryReviewItems";

    let reviewService: InventoryReviewService = sl.resolve(tInventoryReviewService);
    let reviewList: InventoryReviewItems[] = $state([]);
    let isLoading = $state(true);
    function refreshList() {
      isLoading = true;
      reviewService.getItemSheetData(-1).then((res) => {
        reviewList = res;
        console.log(reviewList);
        isLoading = false;
      });
    }
    refreshList();

  </script>
  
  <div class="relative overflow-x-auto p-5">
    <div class="ml-2 mb-2 inline-flex">
      <Button
        hoverColor="#6ff1c4bb"
        borderColor="#57bf9a"
        color="#6ff1c4"
        borderThickness="1"
        on:click={refreshList}
      > Refresh
        <Fa icon={faRefresh} /> 
      </Button>
    </div>
  
    <table class="w-full text-sm text-left rtl:text-right text-gray-500">
      <thead class="text-xs text-gray-700 uppercase bg-gray-50">
        <tr>
          <!-- <th scope="col" class="px-6 py-3"> ItemID</th> -->
          <th scope="col" class="px-6 py-3"> Item Serial</th>
          <th scope="col" class="px-6 py-3"> Item Title</th>
          <th scope="col" class="px-6 py-3"> Invoice Type</th>
          <!-- <th scope="col" class="px-6 py-3"> InvoiceDay</th> -->
          <th scope="col" class="px-6 py-3"> Invoice Date</th>
          <th scope="col" class="px-6 py-3"> Invoice Number</th>
          <th scope="col" class="px-6 py-3"> Invoice Quantity</th>
          <th scope="col" class="px-6 py-3"> Invoice Price</th>
          <th scope="col" class="px-6 py-3"> Invoice Fee</th>
          <th scope="col" class="px-6 py-3"> Running Quantity</th>
        </tr>
      </thead>
      <tbody>
        {#each reviewList as ven}
          <!-- content here -->
          <tr class="bg-white border-b">
              <!-- <td class="px-6 py-4"> {ven.ItemID} </td> -->
            <th scope="row" class="px-6 py-4 font-medium text-gray-900 whitespace-nowrap"> {ven.itemSerial} </th>
            <td class="px-6 py-4"> {ven.itemTitle} </td>
            <td class="px-6 py-4"> {ven.invoiceType} </td>
            <!-- <td class="px-6 py-4"> {ven.InvoiceDay} </td> -->
            <td class="px-6 py-4"> {new Date(ven.invoiceDate).toLocaleString()} </td>
            <td class="px-6 py-4"> {ven.invoiceNumber} </td>
            <td class="px-6 py-4"> {ven.invoiceQuantity} </td>
            <td class="px-6 py-4"> {ven.invoicePrice} </td>
            <td class="px-6 py-4"> {ven.invoiceFee} </td>
            <td class="px-6 py-4"> {ven.runningQuantity} </td>
          </tr>{/each}
      </tbody>
    </table>
    {#if isLoading}
      <div class="flex justify-center items-center mt-4">
        <Fa icon={faSpinner} size="3x" spin />
      </div>
    {/if}
  </div>
  
<style lang="postcss">
.ops {
    svg {
    @apply hover:scale-125 transition-all cursor-pointer;
    }
}
</style>  