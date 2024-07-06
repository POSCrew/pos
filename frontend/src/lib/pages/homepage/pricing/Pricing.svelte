<script lang="ts">
  import {
    faEdit,
    faPlus,
    faRefresh,
    faSpinner,
    faRemove,
    faTrash,
    faArrowLeft,
    faArrowRight,
  } from "@fortawesome/free-solid-svg-icons";
  import {
    Button,
    DatePicker,
    Dialog,
    DialogResult,
    DialogUtils,
    TextField,
  } from "ui-commons";
  import Fa from "svelte-fa";
  import { Pricing } from "../../../data/Pricing";
  import { sl } from "../../../di";
  import type { AxiosError } from "axios";
  import { dialogErrorHandler } from "../../../utils/svelte-utils";
  import {
    tPricingService,
    type PricingService,
  } from "../../../services/PricingService";
  let pricingService: PricingService = sl.resolve(tPricingService);

  let newDialogOpen = $state(false);
  let pricing: Pricing = $state(new Pricing(new Date(), new Date()));

  let pageSize = 10;
  let currentPage: number = $state(1);
  let totalPages: number = $state(1);

  let pricingList: Pricing[] = $state([]);
  let isLoading = $state(true);
  function refreshList() {
    isLoading = true;

    pricingService.getCount().then((res) => {
      totalPages = Math.ceil(res / pageSize);
      if (currentPage > totalPages) currentPage = 0;
    });

    pricingService.getPricings(currentPage - 1, pageSize).then((res) => {
      pricingList = res;
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

  async function onNewPricing() {
    newDialogOpen = true;
    pricing = new Pricing(
      await pricingService.getNewPricingStartDate(),
      new Date(new Date().toLocaleDateString()),
    );
  }
  function onSave() {
    pricingService.create(pricing).then((res) => {
      refreshList();
      newDialogOpen = false;
    });
  }
  function onCancel() {
    newDialogOpen = false;
  }

  function deleteItem() {
    DialogUtils.confirmation(
      `Do you really want to delete last pricing?\n`,
    ).then((res) => {
      if (res === DialogResult.OK) {
        pricingService.removeLastPricing().then(() => {
          refreshList();
        });
      }
    });
  }
</script>

<div class="relative overflow-x-auto p-5">
  <div class="ml-2 mb-2 inline-flex">
    <Button
      hoverColor="#6ff1c4bb"
      borderColor="#57bf9a"
      color="#6ff1c4"
      borderThickness="1"
      on:click={onNewPricing}
    >
      <Fa icon={faPlus} /> New
    </Button>
    <div class="m-1" />
    <Button
      hoverColor="#f16f6fbb"
      borderColor="#cf8c8c"
      color="#ed9a9a"
      borderThickness="1"
      on:click={deleteItem}
    >
      <Fa icon={faRemove} /> Remove Last Pricing
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
        <th scope="col" class="px-6 py-3"> Start Date</th>
        <th scope="col" class="px-6 py-3"> End Date </th>
      </tr>
    </thead>
    <tbody>
      {#each pricingList as ven}
        <!-- content here -->
        <tr class="bg-white border-b">
          <td scope="row" class="px-6 py-4">
            {new Date(ven.startDate).toLocaleString()}
          </td>
          <td class="px-6 py-4"> {new Date(ven.endDate).toLocaleString()} </td>
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
<Dialog bind:isOpen={newDialogOpen}>
  <div class="min-w-96 min-h-60 overflow-y-auto">
    <h2>{pricing.id ? "Edit" : "New"} Pricing:</h2>

    <DatePicker
      label="Start Date :"
      disabled={true}
      value={new Date(pricing.startDate).toISOString().substring(0, 10)}
    />
    <DatePicker label="End Date :" bind:value={pricing.endDate} />
    <div class="h-2"></div>
    <div class="flex">
      <Button on:click={onSave}>Do Pricing</Button>
      <Button on:click={onCancel}>Cancel</Button>
    </div>
  </div>
</Dialog>

<style lang="postcss">
  .ops {
    svg {
      @apply hover:scale-125 transition-all cursor-pointer;
    }
  }
</style>
