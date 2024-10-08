import App from "./App.svelte";
import "./index.css";
import "@fortawesome/fontawesome-free/css/all.css";
import { mount } from "svelte";
import { gsap } from "gsap/dist/gsap";
import { Flip } from "gsap/dist/Flip";

gsap.registerPlugin(Flip);

String.prototype.hashCode = function () {
  var hash = 0,
    i,
    chr;
  if (this.length === 0) return hash;
  for (i = 0; i < this.length; i++) {
    chr = this.charCodeAt(i);
    hash = (hash << 5) - hash + chr;
    hash |= 0; // Convert to 32bit integer
  }
  return hash;
};
let app;
try {
  app = mount(App, { target: document.getElementById("app") });
} catch (e) {}
export default app;
