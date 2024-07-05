<script lang="ts">
  import { faPlus } from "@fortawesome/free-solid-svg-icons";
  import { Button, Dialog, DialogUtils, TextField } from "ui-commons";
  import Fa from "svelte-fa";
  import type { Vendor } from "../../../data/Vendor";
  import { sl } from "../../../di";
  import {
    tVendorService,
    VendorService,
  } from "../../../services/vendorService";
  let vendorService: VendorService = sl.resolve(tVendorService);

  let newDialogOpen = $state(false);
  let vendor: Vendor = $state({
    code: "",
    firstName: "",
    lastName: "",
    address: "",
    phoneNumber: "",
  });

  let vendorList: Vendor[] = $state([]);
  function refreshList() {
    vendorService.getVendors(0, 100).then((res) => {
      vendorList = res;
    });
  }
  refreshList();

  function onNewVendor() {
    newDialogOpen = true;
  }
  function onSave() {
    vendorService.create(vendor).then((res) => {
      refreshList();
      newDialogOpen = false;
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
        </tr>{/each}
    </tbody>
  </table>
</div>
<Dialog isOpen={newDialogOpen}>
  <div class="min-w-96 min-h-60 overflow-y-auto">
    <h2>New Vendor:</h2>

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

<style>
  butto {
    color: #57bf9a;
  }
</style>
