<script lang="ts">
  import { Button, DialogResult, DialogUtils, Space } from "ui-commons";
  import { faHouse, faPlus, faUser } from "@fortawesome/free-solid-svg-icons";
  import Fa from "svelte-fa";
  import NavigationMenu from "./sale-inv/NavigationMenu.svelte";
  import MainArea from "./sale-inv/MainArea.svelte";
  import { onMount } from "svelte";
  import { navigate, Route, Router } from "svelte-navigator";
  import { AuthService, tAuthService } from "../../services/AuthService";
  import type { User } from "../../data/User";
  import { sl } from "../../di";
  import { createTabs, melt } from '@melt-ui/svelte';
  import SaleInvoice from "./sale-inv/SaleInvoice.svelte";
  import Vendor from "./vendor/Vendor.svelte";
  import Customer from "./customer/Customer.svelte";
  import Item from "./item/Item.svelte";
  import InventoryReview from "./InventoryReview/InventoryReview.svelte";
  import SalesReview from "./salesReview/SalesReview.svelte";
  import Pricing from "./pricing/Pricing.svelte";
  import SaleInvoiceList from "./sale-invoice-list/SaleInvoiceList.svelte";
  import PurchaseInvoice from "./purchase-inv/PurchaseInvoice.svelte";
  import PurchaseInvoiceList from "./purchase-invoice-list/PurchaseInvoiceList.svelte";
  
  let authService: AuthService;
  let user: User = $state(null);

  let items = ([]);
  let invoice = ({});
  onMount(async () => {
    authService = sl.resolve(tAuthService);
    user = await authService.currentUser();

    checkIfUserLoggedIn();
  });

  function checkIfUserLoggedIn() {
    if (user == null) {
      navigate("/login");
    }
  }

let tabGroup = $state({ind:0})
function tabIndexChange(ind){
  tabGroup.ind = ind;
}
</script>

<div class="h-full flex flex-col">


<div class="flex-grow-0">
  <div class="flex justify-start">

  
  <div class="tab" on:click={()=>tabIndexChange(0)} data-state="{tabGroup.ind===0?'active':''}">
    Sale
   </div>
  
  <div class="tab" on:click={()=>tabIndexChange(1)} data-state={tabGroup.ind===1?'active':''}>
    Inventory
  </div>
</div>
  <div class="navbar bg-slate-200 h-10 flex items-center px-2 md:px-2 lg:px-2 py-2">
   {#if tabGroup.ind===0}
   
  
    {@render navBtn("Sale invoice", faPlus, ()=>{navigate('sale-inv')})}
    <Space width="4px"/>
    
    {@render navBtn("Customer", faPlus, ()=> {navigate('customer')})}
    <Space width="4px"/>
    
    {#if user?.username == "admin"}
    {@render navBtn("Pricing", faPlus, () => {navigate('pricing')})}
    <Space width="4px"/>
    
    {@render navBtn("Sales Review", faPlus, ()=>{navigate('sale-review')})}
    <Space width="4px"/>
    
    {/if}
   
   {:else if tabGroup.ind===1}
   {@render navBtn("Purchase invoice", faPlus, ()=>{navigate('purchase-inv')})}
   <Space width="4px"/>
   {@render navBtn("Vendor", faPlus, ()=>{navigate('vendor')})}
    <Space width="4px"/>
    {@render navBtn("Item", faPlus, ()=>{navigate('item')})}
    <Space width="4px"/>
    
    {#if user?.username == "admin"}
    {@render navBtn("Inventory Review", faPlus, ()=>{navigate('inv-review')})}
    <Space width="4px"/>
    {/if}
   {/if}

   
   <div class="w-fit ml-auto">
     <Button hoverColor="#fff3" borderColor='#555' borderThickness=1  on:click={()=>{
        DialogUtils.confirmation(
          `Do you want to log out ?\n`,
        ).then((res) => {
          console.log("result : ", res);

          if (res === DialogResult.OK) {
            authService.logout().then(() => {
              navigate('login');
            });
        }
      });
     }}>
       <Fa icon={faUser} /> {user?.username}
      </Button>
    </div>
   
  </div>
</div>

<div class="grow">


  <Route path="sale-inv">
    <SaleInvoice/>
  </Route>
  <Route path="purchase-inv">
    <PurchaseInvoice/>
  </Route>
  <Route path="sale-inv-list">
    <SaleInvoiceList/>
  </Route>
  <Route path="purchase-inv-list">
    <PurchaseInvoiceList/>
  </Route>
  <Route path="customer">
    <Customer/>
  </Route>
  <Route path="item">
    <Item/>
  </Route>
  <Route path="vendor">
    <Vendor/>
  </Route>
  <Route path="inv-review">
    <InventoryReview/>
  </Route>
  <Route path="sale-review">
    <SalesReview/>
  </Route>
  <Route path="pricing">
    <Pricing/>
  </Route>
</div>
</div>

{#snippet navBtn(text, icon, onclick=()=>{})}
<Button hoverColor="#fff3" borderColor='#555' borderThickness=1 on:click={onclick}>
  <Fa icon={icon} /> {text}
</Button>
{/snippet}


<style lang="postcss">
  .navbar{
    @apply border-b-[1px] border-b-slate-600;
  }

  .tab {
    display: flex;
    align-items: center;
    justify-content: center;

    cursor: default;
    user-select: none;
    border-radius: 0;
    background-color: theme(colors.neutral.100);

    color: theme(colors.neutral.900);
    font-weight: 500;
    line-height: 1;

    height: theme(spacing.12);
    padding-inline: theme(spacing.2);

    &:focus {
      position: relative;
    }

    &:focus-visible {
      @apply z-10 ring-2;
    }

    &[data-state='active'] {
      @apply focus:relative text-slate-800;
      background-color: rgb(183, 243, 255);
      text-decoration: underline;
    }

  }
</style>