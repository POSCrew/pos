<script lang="ts">
  import MainArea from "../MainArea.svelte";
  import NavigationMenu from "../NavigationMenu.svelte";

  let items;
</script>

<h1>SaleInvoice</h1>
<main class="flex min-h-full">
  <!-- content here -->
  <div class="flex-grow-0 w-96">
    <NavigationMenu {items} />
  </div>
  <div class="grow w-0">
    <MainArea {items} />
  </div>
</main>
