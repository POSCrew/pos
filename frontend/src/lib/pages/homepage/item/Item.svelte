<script lang="ts">
  import {
    faEdit,
    faPlus,
    faSpinner,
    faRefresh,
    faTrash,
    faArrowLeft,
    faArrowRight,
  } from "@fortawesome/free-solid-svg-icons";
  import {
    Button,
    Dialog,
    DialogResult,
    DialogUtils,
    NumberField,
    TextField,
  } from "ui-commons";
  import Fa from "svelte-fa";
  import { cloneItem, Item } from "../../../data/Item";
  import { sl } from "../../../di";
  import {
    tItemService,
    type ItemService,
  } from "../../../services/ItemService";
  let itemService: ItemService = sl.resolve(tItemService);

  let newDialogOpen = $state(false);
  let item: Item = $state(new Item("", "", "", 0));

  let pageSize = 10;
  let currentPage: number = $state(1);
  let totalPages: number = $state(1);

  let itemList: Item[] = $state([]);
  let isLoading = $state(true);
  function refreshList() {
    isLoading = true;

    itemService.getCount().then((res) => {
      totalPages = Math.ceil(res / pageSize);
      if (currentPage > totalPages) currentPage = 0;
    });

    itemService.getItems(currentPage - 1, pageSize).then((res) => {
      itemList = res;
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

  function onNewItem() {
    newDialogOpen = true;
    item = new Item("", "", "", 0);
  }
  function onSave() {
    (!item.id ? itemService.create(item) : itemService.update(item)).then(
      (res) => {
        refreshList();
        newDialogOpen = false;
      },
    );
  }
  function onCancel() {
    newDialogOpen = false;
  }

  function editItem(ven: Item) {
    item = cloneItem(ven);
    newDialogOpen = true;
  }

  function deleteItem(ven: Item) {
    console.log("delete item ", ven);

    DialogUtils.confirmation(
      `Do you really want to delete this item : '${ven.serial || ""}' '${ven.title || ""}'?\n`,
    ).then((res) => {
      console.log("result : ", res);

      if (res === DialogResult.OK) {
        itemService.remove(ven.id).then(() => {
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
      on:click={onNewItem}
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
              <div on:click={() => editItem(ven)}>
                <Fa
                  icon={faEdit}
                  class="hover:scale-125 transition-all cursor-pointer"
                />
              </div>
              <div on:click={() => deleteItem(ven)}>
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
    <h2>{item.id ? "Edit" : "New"} Item:</h2>

    <TextField
      type="text"
      label="Serial"
      placeholder="Serial"
      bind:value={item.serial}
    />
    <TextField
      type="text"
      label="Title"
      placeholder="Title"
      bind:value={item.title}
    />
    <TextField
      type="text"
      label="Description"
      placeholder="Description"
      bind:value={item.description}
    />
    <NumberField
      label="Sale Price:"
      placeholder="0"
      bind:value={item.salePrice}
    />
    <div class="h-2"></div>
    <div class="flex">
      <Button on:click={onSave}>Save</Button>
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
