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
  import { cloneCustomer, Customer } from "../../../data/Customer";
  import { sl } from "../../../di";
  import { tCustomerService, type CustomerService } from "../../../services/CustomerService";
  let customerService: CustomerService = sl.resolve(tCustomerService);

  let newDialogOpen = $state(false);
  let customer: Customer = $state(new Customer("", "", "", "", ""));

  let customerList: Customer[] = $state([]);
  let isLoading = $state(true);
  function refreshList() {
    isLoading = true;
    customerService.getCustomers(0, 100).then((res) => {
      customerList = res;
      isLoading = false;
    });
  }
  refreshList();

  function onNewCustomer() {
    newDialogOpen = true;
    customer = new Customer("", "", "", "", "");
  }
  function onSave() {
    (!customer.id
      ? customerService.create(customer)
      : customerService.update(customer)
    ).then((res) => {
      refreshList();
      newDialogOpen = false;
    });
  }
  function onCancel() {
    newDialogOpen = false;
  }

  function editItem(ven: Customer) {
    customer = cloneCustomer(ven);
    newDialogOpen = true;
  }

  function deleteItem(ven: Customer) {
    console.log("delete item ", ven);

    DialogUtils.confirmation(
      `Do you really want to delete this item : ${ven.firstName || ""} ${ven.lastName || ''}?\n`,
    ).then((res) => {
      console.log("result : ", res);

      if (res === DialogResult.OK) {
        customerService.remove(ven.id).then(() => {
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
      on:click={onNewCustomer}
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
        <th scope="col" class="px-6 py-3"> Full name</th>
        <th scope="col" class="px-6 py-3"> Code </th>
        <th scope="col" class="px-6 py-3"> Phone Number</th>
        <th scope="col" class="px-6 py-3"> Address </th>
        <th scope="col" class="px-6 py-3"> Operation </th>
      </tr>
    </thead>
    <tbody>
      {#each customerList as ven}
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
    <h2>{customer.id ? "Edit" : "New"} Customer:</h2>

    <TextField
      type="text"
      label="First Name :"
      placeholder="First Name"
      bind:value={customer.firstName}
    />
    <TextField
      type="text"
      label="Last Name :"
      placeholder="Last Name"
      bind:value={customer.lastName}
    />
    <TextField
      type="text"
      label="Code :"
      placeholder="Code"
      bind:value={customer.code}
    />
    <TextField
      type="tel"
      label="phone number :"
      placeholder="Phone number"
      bind:value={customer.phoneNumber}
    />
    <TextField
      type="text"
      label="Address :"
      placeholder="Address"
      bind:value={customer.address}
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
