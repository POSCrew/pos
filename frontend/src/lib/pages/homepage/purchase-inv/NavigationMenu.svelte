<script lang="ts">
  import { TextField } from "ui-commons";
  import type { Item } from "../../../data/Item";
  import type { Vendor } from "../../../data/Vendor";
  import { createEventDispatcher } from "svelte";
  /** @type {{ items: Item[] }} */
  let { items, vendors = [] } = $props<{
    items: Item[];
    vendors: Vendor[];
  }>();

  let dispatch = createEventDispatcher();

  let search = $state("");

  let filteredItems: Item[] = $derived.by(() => {
    console.log(search);

    return items.filter((e: Item) => {
      return (
        e.title?.toLowerCase().includes(search) ||
        e.serial?.toLowerCase().includes(search) ||
        e.description?.toLowerCase().includes(search)
      );
    });
  });

  let filteredVendors: Vendor[] = $derived.by(() => {
    return vendors.filter((e: Vendor) => {
      return (
        e.firstName?.toLowerCase().includes(search) ||
        e.lastName?.toLowerCase().includes(search) ||
        e.code?.toLowerCase().includes(search)
      );
    });
  });

  function onSearchKeyPress(e) {
    switch (e.keyCode) {
      case 13:
        if (filteredItems[0]) {
          dispatch("onItemAdd", filteredItems[0]);
          search = "";
        } else if (filteredVendors[0]) {
          dispatch("onVendorSelected", filteredVendors[0]);
          search = "";
        }
        break;
    }
  }
</script>

<div class="flex flex-col nav-menu bg-gray-100 h-full p-3">
  <div class="overflow-y-auto" style="max-height:80vh;">
    <TextField
      type="text"
      bind:value={search}
      placeholder="Enter something"
      on:keydown={onSearchKeyPress}
    />

    {#each filteredItems as item, i}
      <div
        class="border-[1px] rounded-sm m-1 bg-cyan-100 pl-2"
        on:click={() => {
          dispatch("onItemAdd", filteredItems[i]);
        }}
      >
        <span class="m-0">{item.serial}</span>
        <p class="text-gray-600 mt-[-8px] italic">{item.title}</p>
      </div>
    {/each}
    {#each filteredVendors as vendor, i}
      <div
        class="border-[1px] rounded-sm m-1 bg-violet-100 pl-2"
        on:click={() => {
          dispatch("onVendorSelected", filteredVendors[i]);
        }}
      >
        <span class="m-0">{vendor.firstName} {vendor.lastName}</span>
        <p class="text-gray-600 mt-[-4px]">{vendor.code}</p>
      </div>
    {/each}
  </div>
</div>
