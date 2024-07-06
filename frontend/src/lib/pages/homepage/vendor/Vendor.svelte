<script lang="ts">
  import {
    faEdit,
    faPlus,
    faSpinner,
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
  import { cloneVendor, Vendor } from "../../../data/Vendor";
  import { sl } from "../../../di";
  import {
    tVendorService,
    VendorService,
  } from "../../../services/vendorService";
  let vendorService: VendorService = sl.resolve(tVendorService);

  let newDialogOpen = $state(false);
  let vendor: Vendor = $state(new Vendor("", "", "", "", ""));

  let vendorList: Vendor[] = $state([]);
  let isLoading = $state(true);
  function refreshList() {
    isLoading = true;
    vendorService.getVendors(0, 100).then((res) => {
      vendorList = res;
      isLoading = false;
    });
  }
  refreshList();

  function onNewVendor() {
    newDialogOpen = true;
    vendor = new Vendor("", "", "", "", "");
  }
  function onSave() {
    (!vendor.id
      ? vendorService.create(vendor)
      : vendorService.update(vendor)
    ).then((res) => {
      refreshList();
      newDialogOpen = false;
    });
  }

  function editItem(ven: Vendor) {
    vendor = cloneVendor(ven);
    newDialogOpen = true;
  }

  function deleteItem(ven: Vendor) {
    console.log("delete item ", ven);

    DialogUtils.confirmation(
      `Do you really want to delete this item : ${ven.firstName || ""} ${ven.lastName || ''}?\n`,
    ).then((res) => {
      console.log("result : ", res);

      if (res === DialogResult.OK) {
        vendorService.remove(ven.id).then(() => {
          refreshList();
        });
      }
    });
  }
</script>

<div class="relative overflow-x-auto p-5">
  <div class="ml-2 mb-2">
    <Button
      hoverColor="#6ff1c4bb"
      borderColor="#57bf9a"
      color="#6ff1c4"
      borderThickness="1"
      on:click={onNewVendor}
    >
      <Fa icon={faPlus} /> New
    </Button>
  </div>

  <table class="w-full text-sm text-left rtl:text-right text-gray-500">
    <thead class="text-xs text-gray-700 uppercase bg-gray-50">
      <tr>
        <th scope="col" class="px-6 py-3"> Full name</th>
        <th scope="col" class="px-6 py-3"> Code </th>
        <th scope="col" class="px-6 py-3"> Phone Number</th>
        <th scope="col" class="px-6 py-3"> Address </th>
        <th scope="col" class="px-6 py-3"> Operation </th>
      </tr>
    </thead>
    <tbody>
      {#each vendorList as ven}
        <!-- content here -->

        <tr class="bg-white border-b">
          <th
            scope="row"
            class="px-6 py-4 font-medium text-gray-900 whitespace-nowrap"
          >
            {ven.firstName}
            {ven.lastName}
          </th>
          <td class="px-6 py-4"> {ven.code} </td>
          <td class="px-6 py-4"> {ven.phoneNumber} </td>
          <td class="px-6 py-4"> {ven.address} </td>
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
  {/if}
</div>
<Dialog bind:isOpen={newDialogOpen}>
  <div class="min-w-96 min-h-60 overflow-y-auto">
    <h2>{vendor.id ? "Edit" : "New"} Vendor:</h2>

    <TextField
      type="text"
      label="First Name"
      placeholder="First Name :"
      bind:value={vendor.firstName}
    />
    <TextField
      type="text"
      label="Last Name"
      placeholder="Last Name :"
      bind:value={vendor.lastName}
    />
    <TextField
      type="text"
      label="Enter Code"
      placeholder="Code : "
      bind:value={vendor.code}
    />
    <TextField
      type="text"
      label="Enter phone number"
      placeholder="Phone number : "
      bind:value={vendor.phoneNumber}
    />
    <TextField
      type="text"
      label="Address"
      placeholder="Address : "
      bind:value={vendor.address}
    />
    <div class="h-2"></div>
    <Button on:click={onSave}>Save</Button>
  </div>
</Dialog>

<style lang="postcss">
  .ops {
    svg {
      @apply hover:scale-125 transition-all cursor-pointer;
    }
  }
</style>
