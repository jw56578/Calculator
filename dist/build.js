!function(e){function r(e){for(var r=[],t=0,n=e.length;n>t;t++)-1==p.call(r,e[t])&&r.push(e[t]);return r}function t(e,t,n,o){if("string"!=typeof e)throw"System.register provided no module name";var a;a="boolean"==typeof n?{declarative:!1,deps:t,execute:o,executingRequire:n}:{declarative:!0,deps:t,declare:n},a.name=e,e in c||(c[e]=a),a.deps=r(a.deps),a.normalizedDeps=a.deps}function n(e,r){if(r[e.groupIndex]=r[e.groupIndex]||[],-1==p.call(r[e.groupIndex],e)){r[e.groupIndex].push(e);for(var t=0,o=e.normalizedDeps.length;o>t;t++){var a=e.normalizedDeps[t],l=c[a];if(l&&!l.evaluated){var u=e.groupIndex+(l.declarative!=e.declarative);if(void 0===l.groupIndex||l.groupIndex<u){if(void 0!==l.groupIndex&&(r[l.groupIndex].splice(p.call(r[l.groupIndex],l),1),0==r[l.groupIndex].length))throw new TypeError("Mixed dependency cycle detected");l.groupIndex=u}n(l,r)}}}}function o(e){var r=c[e];r.groupIndex=0;var t=[];n(r,t);for(var o=!!r.declarative==t.length%2,a=t.length-1;a>=0;a--){for(var u=t[a],i=0;i<u.length;i++){var s=u[i];o?l(s):d(s)}o=!o}}function a(e){return f[e]||(f[e]={name:e,dependencies:[],exports:{},importers:[]})}function l(r){if(!r.module){var t=r.module=a(r.name),n=r.module.exports,o=r.declare.call(e,function(e,r){t.locked=!0,n[e]=r;for(var o=0,a=t.importers.length;a>o;o++){var l=t.importers[o];if(!l.locked){var u=p.call(l.dependencies,t);l.setters[u](n)}}return t.locked=!1,r});if(t.setters=o.setters,t.execute=o.execute,!t.setters||!t.execute)throw new TypeError("Invalid System.register form for "+r.name);for(var u=0,d=r.normalizedDeps.length;d>u;u++){var i,v=r.normalizedDeps[u],m=c[v],g=f[v];g?i=g.exports:m&&!m.declarative?i=m.module.exports&&m.module.exports.__esModule?m.module.exports:{"default":m.module.exports,__useDefault:!0}:m?(l(m),g=m.module,i=g.exports):i=s(v),g&&g.importers?(g.importers.push(t),t.dependencies.push(g)):t.dependencies.push(null),t.setters[u]&&t.setters[u](i)}}}function u(e){var r,t=c[e];if(t)t.declarative?i(e,[]):t.evaluated||d(t),r=t.module.exports;else if(r=s(e),!r)throw new Error("Unable to load dependency "+e+".");return(!t||t.declarative)&&r&&r.__useDefault?r["default"]:r}function d(r){if(!r.module){var t={},n=r.module={exports:t,id:r.name};if(!r.executingRequire)for(var o=0,a=r.normalizedDeps.length;a>o;o++){var l=r.normalizedDeps[o],i=c[l];i&&d(i)}r.evaluated=!0;var s=r.execute.call(e,function(e){for(var t=0,n=r.deps.length;n>t;t++)if(r.deps[t]==e)return u(r.normalizedDeps[t]);throw new TypeError("Module "+e+" not declared as a dependency.")},t,n);s&&(n.exports=s)}}function i(r,t){var n=c[r];if(n&&!n.evaluated&&n.declarative){t.push(r);for(var o=0,a=n.normalizedDeps.length;a>o;o++){var l=n.normalizedDeps[o];-1==p.call(t,l)&&(c[l]?i(l,t):s(l))}n.evaluated||(n.evaluated=!0,n.module.execute.call(e))}}function s(e){if(v[e])return v[e];var r=c[e];if(!r)throw"Module "+e+" not present.";o(e),i(e,[]),c[e]=void 0;var t=r.module.exports;return(!t||!r.declarative&&t.__esModule!==!0)&&(t={"default":t,__useDefault:!0}),v[e]=t}var c={},p=Array.prototype.indexOf||function(e){for(var r=0,t=this.length;t>r;r++)if(this[r]===e)return r;return-1},f={},v={};return function(r,n){var o,o={register:t,get:s,set:function(e,r){v[e]=r},newModule:function(e){return e},global:e};o.set("@empty",{}),n(o);for(var a=0;a<r.length;a++)s(r[a])}}("undefined"!=typeof window?window:global)(["src/main"],function(e){e.register("src/greeter",[],function(e){"use strict";function r(){alert(t)}var t;return e("sayHello",r),{setters:[],execute:function(){t="hello"}}}),e.register("src/main",["src/greeter"],function(e){"use strict";var r;return{setters:[function(e){r=e.sayHello}],execute:function(){r()}}})});