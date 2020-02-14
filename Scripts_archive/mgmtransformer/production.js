steal.plugins("jquery/controller","jquery/controller/subscribe","jquery/view/ejs","jquery/controller/view","jquery/model","jquery/dom/fixture","jquery/dom/form_params","jquery/dom/cookie").css("mgmtransformer").resources("jquery.maskedinput.js","jshashtable-2.1.js","jquery.numberformatter-1.2.1.js","date.format.js","jquery.validate.js","additional-methods.js").models("stock","quote","quotedetail","stockphase","stockconfiguration","stockkva","stockcatalog","customcatalog","customer").controllers("stock",
"quote","newquote","quotedetail","customer").views("//mgmtransformer/views/newquote/configs.ejs","//mgmtransformer/views/newquote/init.ejs","//mgmtransformer/views/newquote/kva.ejs","//mgmtransformer/views/newquote/phase.ejs","//mgmtransformer/views/quote/edit.ejs","//mgmtransformer/views/quote/init.ejs","//mgmtransformer/views/quote/list.ejs","//mgmtransformer/views/quote/show.ejs","//mgmtransformer/views/quotedetail/edit.ejs","//mgmtransformer/views/quotedetail/init.ejs","//mgmtransformer/views/quotedetail/list.ejs",
"//mgmtransformer/views/quotedetail/show.ejs","//mgmtransformer/views/quotedetail/empty.ejs","//mgmtransformer/views/quotedetail/kitlist.ejs","//mgmtransformer/views/quotedetail/kitshow.ejs","//mgmtransformer/views/stock/edit.ejs","//mgmtransformer/views/stock/init.ejs","//mgmtransformer/views/stock/list.ejs","//mgmtransformer/views/stock/show.ejs","//mgmtransformer/views/customer/init.ejs","//mgmtransformer/views/customer/edit.ejs","//mgmtransformer/views/customer/show.ejs","//mgmtransformer/views/loader/loader.ejs").then(function(a){function e(){clearTimeout(h);
h=setTimeout(f,i)}function d(b,c,j){a("#maincontent > div.tab").hide();a("ul.navigation li.active").removeClass("active");a(b).addClass("active");a(a(b).find("a").attr("href")).show();c||a("#quote").mgmtransformer_quote("displaylist");if(a(b).hasClass("newquote")){a("#newquote").mgmtransformer_newquote("reset");j&&a("#newquote #btn-gotoquote").click()}}var i=6E5,f=function(){window.location="login.aspx?logout=timeout"},h=setTimeout(f,i);a(".navigation").find(".tab").click(function(){d(this,false)}).end().find(".cart").click(function(){d(a(".navigation .tab a[href=#newquote]").parent(),
false,true)});a(".logout a").ajaxComplete(function(b,c){a.parseJSON(c.responseText).error=="timeout"&&f();e()});a("body").click(e).keypress(e);a("#stock").mgmtransformer_stock(a.parseJSON(a("#stockoptions").val()));a("#quote").mgmtransformer_quote({isHistory:true}).bind("return",function(){a(this).hide();a("#newquote").show()}).bind("finalized",function(){a("#newquote").mgmtransformer_newquote("finalized");d(a(".navigation .tab a[href=#quote]").parent(),true)}).bind("copyquote",function(b,c){a("#newquote").mgmtransformer_newquote("copyquote",
c)});a("#newquote").mgmtransformer_newquote(a.parseJSON(a("#newquoteoptions").val())).bind("showdetail",function(b,c){if(c!=0){a("#quote").mgmtransformer_quote("showcart",c);a(this).hide()}}).bind("copied",function(){d(a(".navigation .tab a[href=#newquote]").parent(),false,true)});var g=a("#quoteID").val();if(g!=0&&!a.cookie("load")){Mgmtransformer.Models.Quote.clear({quoteid:g},function(b){a("#quoteID").val(b.quoteid)});a.cookie("load",true,{path:"/"})}else g!=0&&a(".cart").show();a(".navigation a[href=#quote]").click()});
;
steal.end();
steal.plugins("jquery/class","jquery/lang","jquery/event/destroyed").then(function(e){var u=function(a,b,c){var d,f=a.bind&&a.unbind?a:e(j(a)?[a]:a);if(b.indexOf(">")===0){b=b.substr(1);d=function(g){g.target===a&&c.apply(this,arguments)}}f.bind(b,d||c);return function(){f.unbind(b,d||c);a=b=c=d=null}},p=e.makeArray,v=e.isArray,j=e.isFunction,k=e.extend,q=e.String,w=function(a,b,c,d){e(a).delegate(b,c,d);return function(){e(a).undelegate(b,c,d);a=c=d=b=null}},r=function(a,b,c,d){return d?w(a,d,b,
c):u(a,b,c)},l=function(a){return function(){return a.apply(null,[this.nodeName?e(this):this].concat(Array.prototype.slice.call(arguments,0)))}},x=/\./g,y=/_?controllers?/ig,s=function(a){return q.underscore(a.replace("jQuery.","").replace(x,"_").replace(y,""))},z=/[^\w]/,t=/\{([^\}]+)\}/g,A=/^(?:(.*?)\s)?([\w\.\:>]+)$/,m,n=function(a,b){return e.data(a,"controllers",b)};e.Class("jQuery.Controller",{init:function(){if(!(!this.shortName||this.fullName=="jQuery.Controller")){this._fullName=s(this.fullName);
this._shortName=s(this.shortName);var a=this,b=this.pluginName||this._fullName,c;e.fn[b]||(e.fn[b]=function(d){var f=p(arguments),g=typeof d=="string"&&j(a.prototype[d]),B=f[0];return this.each(function(){var h=n(this);if(h=h&&h[b])g?h[B].apply(h,f.slice(1)):h.update.apply(h,f);else a.newInstance.apply(a,[this].concat(f))})});this.actions={};for(c in this.prototype)if(!(c=="constructor"||!j(this.prototype[c])))if(this._isAction(c))this.actions[c]=this._action(c);this.onDocument&&new a(document.documentElement)}},
hookup:function(a){return new this(a)},_isAction:function(a){return z.test(a)?true:e.inArray(a,this.listensTo)>-1||e.event.special[a]||o[a]},_action:function(a,b){t.lastIndex=0;if(!b&&t.test(a))return null;a=b?q.sub(a,[b,window]):a;b=v(a);var c=(b?a[1]:a).match(A);return{processor:o[c[2]]||m,parts:c,delegate:b?a[0]:undefined}},processors:{},listensTo:[],defaults:{}},{setup:function(a,b){var c,d=this.Class;a=a.jquery?a[0]:a;this.element=e(a).addClass(d._fullName);(n(a)||n(a,{}))[d._fullName]=this;
this._bindings=[];this.options=k(k(true,{},d.defaults),b);for(c in d.actions)if(d.actions.hasOwnProperty(c)){b=d.actions[c]||d._action(c,this.options);this._bindings.push(b.processor(b.delegate||a,b.parts[2],b.parts[1],this.callback(c),this))}this.called="init";var f=l(this.callback("destroy"));this.element.bind("destroyed",f);this._bindings.push(function(){e(a).unbind("destroyed",f)});return this.element},bind:function(a,b,c){if(typeof a=="string"){c=b;b=a;a=this.element}return this._binder(a,b,
c)},_binder:function(a,b,c,d){if(typeof c=="string")c=l(this.callback(c));this._bindings.push(r(a,b,c,d));return this._bindings.length},delegate:function(a,b,c,d){if(typeof a=="string"){d=c;c=b;b=a;a=this.element}return this._binder(a,c,d,b)},update:function(a){k(this.options,a)},destroy:function(){if(this._destroyed)throw this.Class.shortName+" controller instance has been deleted";var a=this,b=this.Class._fullName;this._destroyed=true;this.element.removeClass(b);e.each(this._bindings,function(c,
d){d(a.element[0])});delete this._actions;delete this.element.data("controllers")[b];e(this).triggerHandler("destroyed");this.element=null},find:function(a){return this.element.find(a)},_set_called:true});var o=e.Controller.processors;m=function(a,b,c,d,f){var g=f.Class;if(g.onDocument&&!/^Main(Controller)?$/.test(g.shortName)&&a===f.element[0])c=c?"#"+g._shortName+" "+c:"#"+g._shortName;return r(a,b,l(d),c)};e.each("change click contextmenu dblclick keydown keyup keypress mousedown mousemove mouseout mouseover mouseup reset resize scroll select submit focusin focusout mouseenter mouseleave".split(" "),
function(a,b){o[b]=m});var i,C=function(a,b){for(i=0;i<b.length;i++)if(typeof b[i]=="string"?a.Class._shortName==b[i]:a instanceof b[i])return true;return false};e.fn.controllers=function(){var a=p(arguments),b=[],c,d,f;this.each(function(){c=e.data(this,"controllers");for(f in c)if(c.hasOwnProperty(f)){d=c[f];if(!a.length||C(d,a))b.push(d)}});return b};e.fn.controller=function(){return this.controllers.apply(this,arguments)[0]}});
;
steal.end();
steal.plugins("jquery","jquery/lang").then(function(i){var k=false,p=i.makeArray,q=i.isFunction,m=i.isArray,n=i.extend,r=function(a,c){return a.concat(p(c))},t=/xyz/.test(function(){})?/\b_super\b/:/.*/,s=function(a,c,d){d=d||a;for(var b in a)d[b]=q(a[b])&&q(c[b])&&t.test(a[b])?function(g,h){return function(){var f=this._super,e;this._super=c[g];e=h.apply(this,arguments);this._super=f;return e}}(b,a[b]):a[b]},j=i.Class=function(){arguments.length&&j.extend.apply(j,arguments)};n(j,{callback:function(a){var c=
p(arguments),d;a=c.shift();m(a)||(a=[a]);d=this;return function(){for(var b=r(c,arguments),g,h=a.length,f=0,e;f<h;f++)if(e=a[f]){if((g=typeof e=="string")&&d._set_called)d.called=e;b=(g?d[e]:e).apply(d,b||[]);if(f<h-1)b=!m(b)||b._use_call?[b]:b}return b}},getObject:i.String.getObject,newInstance:function(){var a=this.rawInstance(),c;if(a.setup)c=a.setup.apply(a,arguments);if(a.init)a.init.apply(a,m(c)?c:arguments);return a},setup:function(a){this.defaults=n(true,{},a.defaults,this.defaults);return arguments},
rawInstance:function(){k=true;var a=new this;k=false;return a},extend:function(a,c,d){function b(){if(!k)return this.constructor!==b&&arguments.length?arguments.callee.extend.apply(arguments.callee,arguments):this.Class.newInstance.apply(this.Class,arguments)}if(typeof a!="string"){d=c;c=a;a=null}if(!d){d=c;c=null}d=d||{};var g=this,h=this.prototype,f,e,l,o;k=true;o=new this;k=false;s(d,h,o);for(f in this)if(this.hasOwnProperty(f))b[f]=this[f];s(c,this,b);if(a){l=a.split(/\./);e=l.pop();l=h=j.getObject(l.join("."),
window,true);h[e]=b}n(b,{prototype:o,namespace:l,shortName:e,constructor:b,fullName:a});b.prototype.Class=b.prototype.constructor=b;g=b.setup.apply(b,r([g],arguments));if(b.init)b.init.apply(b,g||[]);return b}});j.prototype.callback=j.callback})();
;
steal.end();
(function(E,v){function wa(a,b,d){if(d===v&&a.nodeType===1){d="data-"+b.replace(gb,"$1-$2").toLowerCase();d=a.getAttribute(d);if(typeof d==="string"){try{d=d==="true"?true:d==="false"?false:d==="null"?null:!c.isNaN(d)?parseFloat(d):hb.test(d)?c.parseJSON(d):d}catch(e){}c.data(a,b,d)}else d=v}return d}function ia(a){for(var b in a)if(b!=="toJSON")return false;return true}function xa(a,b,d){var e=b+"defer",f=b+"queue",g=b+"mark",i=c.data(a,e,v,true);if(i&&(d==="queue"||!c.data(a,f,v,true))&&(d==="mark"||
!c.data(a,g,v,true)))setTimeout(function(){if(!c.data(a,f,v,true)&&!c.data(a,g,v,true)){c.removeData(a,e,true);i.resolve()}},0)}function Q(){return false}function aa(){return true}function ya(a,b,d){var e=c.extend({},d[0]);e.type=a;e.originalEvent={};e.liveFired=v;c.event.handle.call(b,e);e.isDefaultPrevented()&&d[0].preventDefault()}function ib(a){var b,d,e,f,g,i,l,m,n,r,A,B=[];f=[];g=c._data(this,"events");if(!(a.liveFired===this||!g||!g.live||a.target.disabled||a.button&&a.type==="click")){if(a.namespace)A=
new RegExp("(^|\\.)"+a.namespace.split(".").join("\\.(?:.*\\.)?")+"(\\.|$)");a.liveFired=this;var C=g.live.slice(0);for(l=0;l<C.length;l++){g=C[l];g.origType.replace(ja,"")===a.type?f.push(g.selector):C.splice(l--,1)}f=c(a.target).closest(f,a.currentTarget);m=0;for(n=f.length;m<n;m++){r=f[m];for(l=0;l<C.length;l++){g=C[l];if(r.selector===g.selector&&(!A||A.test(g.namespace))&&!r.elem.disabled){i=r.elem;e=null;if(g.preType==="mouseenter"||g.preType==="mouseleave"){a.type=g.preType;if((e=c(a.relatedTarget).closest(g.selector)[0])&&
c.contains(i,e))e=i}if(!e||e!==i)B.push({elem:i,handleObj:g,level:r.level})}}}m=0;for(n=B.length;m<n;m++){f=B[m];if(d&&f.level>d)break;a.currentTarget=f.elem;a.data=f.handleObj.data;a.handleObj=f.handleObj;A=f.handleObj.origHandler.apply(f.elem,arguments);if(A===false||a.isPropagationStopped()){d=f.level;if(A===false)b=false;if(a.isImmediatePropagationStopped())break}}return b}}function ba(a,b){return(a&&a!=="*"?a+".":"")+b.replace(jb,"`").replace(kb,"&")}function za(a){return!a||!a.parentNode||a.parentNode.nodeType===
11}function Aa(a,b,d){b=b||0;if(c.isFunction(b))return c.grep(a,function(f,g){return!!b.call(f,g,f)===d});else if(b.nodeType)return c.grep(a,function(f){return f===b===d});else if(typeof b==="string"){var e=c.grep(a,function(f){return f.nodeType===1});if(lb.test(b))return c.filter(b,e,!d);else b=c.filter(b,e)}return c.grep(a,function(f){return c.inArray(f,b)>=0===d})}function mb(a){return c.nodeName(a,"table")?a.getElementsByTagName("tbody")[0]||a.appendChild(a.ownerDocument.createElement("tbody")):
a}function Ba(a,b){if(!(b.nodeType!==1||!c.hasData(a))){var d=c.expando,e=c.data(a),f=c.data(b,e);if(e=e[d]){a=e.events;f=f[d]=c.extend({},e);if(a){delete f.handle;f.events={};for(var g in a){d=0;for(e=a[g].length;d<e;d++)c.event.add(b,g+(a[g][d].namespace?".":"")+a[g][d].namespace,a[g][d],a[g][d].data)}}}}}function Ca(a,b){var d;if(b.nodeType===1){b.clearAttributes&&b.clearAttributes();b.mergeAttributes&&b.mergeAttributes(a);d=b.nodeName.toLowerCase();if(d==="object")b.outerHTML=a.outerHTML;else if(d===
"input"&&(a.type==="checkbox"||a.type==="radio")){if(a.checked)b.defaultChecked=b.checked=a.checked;if(b.value!==a.value)b.value=a.value}else if(d==="option")b.selected=a.defaultSelected;else if(d==="input"||d==="textarea")b.defaultValue=a.defaultValue;b.removeAttribute(c.expando)}}function ca(a){return"getElementsByTagName"in a?a.getElementsByTagName("*"):"querySelectorAll"in a?a.querySelectorAll("*"):[]}function Da(a){if(a.type==="checkbox"||a.type==="radio")a.defaultChecked=a.checked}function Ea(a){if(c.nodeName(a,
"input"))Da(a);else a.getElementsByTagName&&c.grep(a.getElementsByTagName("input"),Da)}function nb(a,b){b.src?c.ajax({url:b.src,async:false,dataType:"script"}):c.globalEval((b.text||b.textContent||b.innerHTML||"").replace(ob,"/*$0*/"));b.parentNode&&b.parentNode.removeChild(b)}function Fa(a,b,d){var e=b==="width"?a.offsetWidth:a.offsetHeight;if(d==="border")return e;c.each(b==="width"?pb:qb,function(){d||(e-=parseFloat(c.css(a,"padding"+this))||0);if(d==="margin")e+=parseFloat(c.css(a,"margin"+this))||
0;else e-=parseFloat(c.css(a,"border"+this+"Width"))||0});return e}function Ga(a){return function(b,d){if(typeof b!=="string"){d=b;b="*"}if(c.isFunction(d)){b=b.toLowerCase().split(Ha);for(var e=0,f=b.length,g,i;e<f;e++){g=b[e];if(i=/^\+/.test(g))g=g.substr(1)||"*";g=a[g]=a[g]||[];g[i?"unshift":"push"](d)}}}}function da(a,b,d,e,f,g){f=f||b.dataTypes[0];g=g||{};g[f]=true;f=a[f];for(var i=0,l=f?f.length:0,m=a===ka,n;i<l&&(m||!n);i++){n=f[i](b,d,e);if(typeof n==="string")if(!m||g[n])n=v;else{b.dataTypes.unshift(n);
n=da(a,b,d,e,n,g)}}if((m||!n)&&!g["*"])n=da(a,b,d,e,"*",g);return n}function la(a,b,d,e){if(c.isArray(b))c.each(b,function(g,i){d||rb.test(a)?e(a,i):la(a+"["+(typeof i==="object"||c.isArray(i)?g:"")+"]",i,d,e)});else if(!d&&b!=null&&typeof b==="object")for(var f in b)la(a+"["+f+"]",b[f],d,e);else e(a,b)}function sb(a,b,d){var e=a.contents,f=a.dataTypes,g=a.responseFields,i,l,m,n;for(l in g)if(l in d)b[g[l]]=d[l];for(;f[0]==="*";){f.shift();if(i===v)i=a.mimeType||b.getResponseHeader("content-type")}if(i)for(l in e)if(e[l]&&
e[l].test(i)){f.unshift(l);break}if(f[0]in d)m=f[0];else{for(l in d){if(!f[0]||a.converters[l+" "+f[0]]){m=l;break}n||(n=l)}m=m||n}if(m){m!==f[0]&&f.unshift(m);return d[m]}}function tb(a,b){if(a.dataFilter)b=a.dataFilter(b,a.dataType);var d=a.dataTypes,e={},f,g,i=d.length,l,m=d[0],n,r,A,B,C;for(f=1;f<i;f++){if(f===1)for(g in a.converters)if(typeof g==="string")e[g.toLowerCase()]=a.converters[g];n=m;m=d[f];if(m==="*")m=n;else if(n!=="*"&&n!==m){r=n+" "+m;A=e[r]||e["* "+m];if(!A){C=v;for(B in e){l=
B.split(" ");if(l[0]===n||l[0]==="*")if(C=e[l[1]+" "+m]){B=e[B];if(B===true)A=C;else if(C===true)A=B;break}}}A||C||c.error("No conversion from "+r.replace(" "," to "));if(A!==true)b=A?A(b):C(B(b))}}return b}function Ia(){try{return new E.XMLHttpRequest}catch(a){}}function ub(){try{return new E.ActiveXObject("Microsoft.XMLHTTP")}catch(a){}}function Ja(){setTimeout(vb,0);return ea=c.now()}function vb(){ea=v}function V(a,b){var d={};c.each(Ka.concat.apply([],Ka.slice(0,b)),function(){d[this]=a});return d}
function La(a){if(!ma[a]){var b=c("<"+a+">").appendTo("body"),d=b.css("display");b.remove();if(d==="none"||d===""){if(!P){P=x.createElement("iframe");P.frameBorder=P.width=P.height=0}x.body.appendChild(P);if(!Z||!P.createElement){Z=(P.contentWindow||P.contentDocument).document;Z.write("<!doctype><html><body></body></html>")}b=Z.createElement(a);Z.body.appendChild(b);d=c.css(b,"display");x.body.removeChild(P)}ma[a]=d}return ma[a]}function na(a){return c.isWindow(a)?a:a.nodeType===9?a.defaultView||
a.parentWindow:false}var x=E.document,wb=E.navigator,xb=E.location,c=function(){function a(){if(!b.isReady){try{x.documentElement.doScroll("left")}catch(k){setTimeout(a,1);return}b.ready()}}var b=function(k,s){return new b.fn.init(k,s,f)},d=E.jQuery,e=E.$,f,g=/^(?:[^<]*(<[\w\W]+>)[^>]*$|#([\w\-]*)$)/,i=/\S/,l=/^\s+/,m=/\s+$/,n=/\d/,r=/^<(\w+)\s*\/?>(?:<\/\1>)?$/,A=/^[\],:{}\s]*$/,B=/\\(?:["\\\/bfnrt]|u[0-9a-fA-F]{4})/g,C=/"[^"\\\n\r]*"|true|false|null|-?\d+(?:\.\d*)?(?:[eE][+\-]?\d+)?/g,G=/(?:^|:|,)(?:\s*\[)+/g,
R=/(webkit)[ \/]([\w.]+)/,I=/(opera)(?:.*version)?[ \/]([\w.]+)/,M=/(msie) ([\w.]+)/,O=/(mozilla)(?:.*? rv:([\w.]+))?/,h=wb.userAgent,j,o,p=Object.prototype.toString,q=Object.prototype.hasOwnProperty,t=Array.prototype.push,y=Array.prototype.slice,w=String.prototype.trim,D=Array.prototype.indexOf,L={};b.fn=b.prototype={constructor:b,init:function(k,s,u){var z;if(!k)return this;if(k.nodeType){this.context=this[0]=k;this.length=1;return this}if(k==="body"&&!s&&x.body){this.context=x;this[0]=x.body;this.selector=
k;this.length=1;return this}if(typeof k==="string")if((z=k.charAt(0)==="<"&&k.charAt(k.length-1)===">"&&k.length>=3?[null,k,null]:g.exec(k))&&(z[1]||!s))if(z[1]){u=(s=s instanceof b?s[0]:s)?s.ownerDocument||s:x;if(k=r.exec(k))if(b.isPlainObject(s)){k=[x.createElement(k[1])];b.fn.attr.call(k,s,true)}else k=[u.createElement(k[1])];else{k=b.buildFragment([z[1]],[u]);k=(k.cacheable?b.clone(k.fragment):k.fragment).childNodes}return b.merge(this,k)}else{if((s=x.getElementById(z[2]))&&s.parentNode){if(s.id!==
z[2])return u.find(k);this.length=1;this[0]=s}this.context=x;this.selector=k;return this}else return!s||s.jquery?(s||u).find(k):this.constructor(s).find(k);else if(b.isFunction(k))return u.ready(k);if(k.selector!==v){this.selector=k.selector;this.context=k.context}return b.makeArray(k,this)},selector:"",jquery:"1.6.1",length:0,size:function(){return this.length},toArray:function(){return y.call(this,0)},get:function(k){return k==null?this.toArray():k<0?this[this.length+k]:this[k]},pushStack:function(k,
s,u){var z=this.constructor();b.isArray(k)?t.apply(z,k):b.merge(z,k);z.prevObject=this;z.context=this.context;if(s==="find")z.selector=this.selector+(this.selector?" ":"")+u;else if(s)z.selector=this.selector+"."+s+"("+u+")";return z},each:function(k,s){return b.each(this,k,s)},ready:function(k){b.bindReady();j.done(k);return this},eq:function(k){return k===-1?this.slice(k):this.slice(k,+k+1)},first:function(){return this.eq(0)},last:function(){return this.eq(-1)},slice:function(){return this.pushStack(y.apply(this,
arguments),"slice",y.call(arguments).join(","))},map:function(k){return this.pushStack(b.map(this,function(s,u){return k.call(s,u,s)}))},end:function(){return this.prevObject||this.constructor(null)},push:t,sort:[].sort,splice:[].splice};b.fn.init.prototype=b.fn;b.extend=b.fn.extend=function(){var k,s,u,z,H,F=arguments[0]||{},J=1,K=arguments.length,oa=false;if(typeof F==="boolean"){oa=F;F=arguments[1]||{};J=2}if(typeof F!=="object"&&!b.isFunction(F))F={};if(K===J){F=this;--J}for(;J<K;J++)if((k=arguments[J])!=
null)for(s in k){u=F[s];z=k[s];if(F!==z)if(oa&&z&&(b.isPlainObject(z)||(H=b.isArray(z)))){if(H){H=false;u=u&&b.isArray(u)?u:[]}else u=u&&b.isPlainObject(u)?u:{};F[s]=b.extend(oa,u,z)}else if(z!==v)F[s]=z}return F};b.extend({noConflict:function(k){if(E.$===b)E.$=e;if(k&&E.jQuery===b)E.jQuery=d;return b},isReady:false,readyWait:1,holdReady:function(k){if(k)b.readyWait++;else b.ready(true)},ready:function(k){if(k===true&&!--b.readyWait||k!==true&&!b.isReady){if(!x.body)return setTimeout(b.ready,1);b.isReady=
true;if(!(k!==true&&--b.readyWait>0)){j.resolveWith(x,[b]);b.fn.trigger&&b(x).trigger("ready").unbind("ready")}}},bindReady:function(){if(!j){j=b._Deferred();if(x.readyState==="complete")return setTimeout(b.ready,1);if(x.addEventListener){x.addEventListener("DOMContentLoaded",o,false);E.addEventListener("load",b.ready,false)}else if(x.attachEvent){x.attachEvent("onreadystatechange",o);E.attachEvent("onload",b.ready);var k=false;try{k=E.frameElement==null}catch(s){}x.documentElement.doScroll&&k&&a()}}},
isFunction:function(k){return b.type(k)==="function"},isArray:Array.isArray||function(k){return b.type(k)==="array"},isWindow:function(k){return k&&typeof k==="object"&&"setInterval"in k},isNaN:function(k){return k==null||!n.test(k)||isNaN(k)},type:function(k){return k==null?String(k):L[p.call(k)]||"object"},isPlainObject:function(k){if(!k||b.type(k)!=="object"||k.nodeType||b.isWindow(k))return false;if(k.constructor&&!q.call(k,"constructor")&&!q.call(k.constructor.prototype,"isPrototypeOf"))return false;
var s;for(s in k);return s===v||q.call(k,s)},isEmptyObject:function(k){for(var s in k)return false;return true},error:function(k){throw k;},parseJSON:function(k){if(typeof k!=="string"||!k)return null;k=b.trim(k);if(E.JSON&&E.JSON.parse)return E.JSON.parse(k);if(A.test(k.replace(B,"@").replace(C,"]").replace(G,"")))return(new Function("return "+k))();b.error("Invalid JSON: "+k)},parseXML:function(k,s,u){if(E.DOMParser){u=new DOMParser;s=u.parseFromString(k,"text/xml")}else{s=new ActiveXObject("Microsoft.XMLDOM");
s.async="false";s.loadXML(k)}u=s.documentElement;if(!u||!u.nodeName||u.nodeName==="parsererror")b.error("Invalid XML: "+k);return s},noop:function(){},globalEval:function(k){if(k&&i.test(k))(E.execScript||function(s){E.eval.call(E,s)})(k)},nodeName:function(k,s){return k.nodeName&&k.nodeName.toUpperCase()===s.toUpperCase()},each:function(k,s,u){var z,H=0,F=k.length,J=F===v||b.isFunction(k);if(u)if(J)for(z in k){if(s.apply(k[z],u)===false)break}else for(;H<F;){if(s.apply(k[H++],u)===false)break}else if(J)for(z in k){if(s.call(k[z],
z,k[z])===false)break}else for(;H<F;)if(s.call(k[H],H,k[H++])===false)break;return k},trim:w?function(k){return k==null?"":w.call(k)}:function(k){return k==null?"":k.toString().replace(l,"").replace(m,"")},makeArray:function(k,s){s=s||[];if(k!=null){var u=b.type(k);k.length==null||u==="string"||u==="function"||u==="regexp"||b.isWindow(k)?t.call(s,k):b.merge(s,k)}return s},inArray:function(k,s){if(D)return D.call(s,k);for(var u=0,z=s.length;u<z;u++)if(s[u]===k)return u;return-1},merge:function(k,s){var u=
k.length,z=0;if(typeof s.length==="number")for(var H=s.length;z<H;z++)k[u++]=s[z];else for(;s[z]!==v;)k[u++]=s[z++];k.length=u;return k},grep:function(k,s,u){var z=[],H;u=!!u;for(var F=0,J=k.length;F<J;F++){H=!!s(k[F],F);u!==H&&z.push(k[F])}return z},map:function(k,s,u){var z,H,F=[],J=0,K=k.length;if(k instanceof b||K!==v&&typeof K==="number"&&(K>0&&k[0]&&k[K-1]||K===0||b.isArray(k)))for(;J<K;J++){z=s(k[J],J,u);if(z!=null)F[F.length]=z}else for(H in k){z=s(k[H],H,u);if(z!=null)F[F.length]=z}return F.concat.apply([],
F)},guid:1,proxy:function(k,s){if(typeof s==="string"){var u=k[s];s=k;k=u}if(!b.isFunction(k))return v;var z=y.call(arguments,2);u=function(){return k.apply(s,z.concat(y.call(arguments)))};u.guid=k.guid=k.guid||u.guid||b.guid++;return u},access:function(k,s,u,z,H,F){var J=k.length;if(typeof s==="object"){for(var K in s)b.access(k,K,s[K],z,H,u);return k}if(u!==v){z=!F&&z&&b.isFunction(u);for(K=0;K<J;K++)H(k[K],s,z?u.call(k[K],K,H(k[K],s)):u,F);return k}return J?H(k[0],s):v},now:function(){return(new Date).getTime()},
uaMatch:function(k){k=k.toLowerCase();k=R.exec(k)||I.exec(k)||M.exec(k)||k.indexOf("compatible")<0&&O.exec(k)||[];return{browser:k[1]||"",version:k[2]||"0"}},sub:function(){function k(u,z){return new k.fn.init(u,z)}b.extend(true,k,this);k.superclass=this;k.fn=k.prototype=this();k.fn.constructor=k;k.sub=this.sub;k.fn.init=function(u,z){if(z&&z instanceof b&&!(z instanceof k))z=k(z);return b.fn.init.call(this,u,z,s)};k.fn.init.prototype=k.fn;var s=k(x);return k},browser:{}});b.each("Boolean Number String Function Array Date RegExp Object".split(" "),
function(k,s){L["[object "+s+"]"]=s.toLowerCase()});h=b.uaMatch(h);if(h.browser){b.browser[h.browser]=true;b.browser.version=h.version}if(b.browser.webkit)b.browser.safari=true;if(i.test("\u00a0")){l=/^[\s\xA0]+/;m=/[\s\xA0]+$/}f=b(x);if(x.addEventListener)o=function(){x.removeEventListener("DOMContentLoaded",o,false);b.ready()};else if(x.attachEvent)o=function(){if(x.readyState==="complete"){x.detachEvent("onreadystatechange",o);b.ready()}};return b}(),pa="done fail isResolved isRejected promise then always pipe".split(" "),
Ma=[].slice;c.extend({_Deferred:function(){var a=[],b,d,e,f={done:function(){if(!e){var g=arguments,i,l,m,n,r;if(b){r=b;b=0}i=0;for(l=g.length;i<l;i++){m=g[i];n=c.type(m);if(n==="array")f.done.apply(f,m);else n==="function"&&a.push(m)}r&&f.resolveWith(r[0],r[1])}return this},resolveWith:function(g,i){if(!e&&!b&&!d){i=i||[];d=1;try{for(;a[0];)a.shift().apply(g,i)}finally{b=[g,i];d=0}}return this},resolve:function(){f.resolveWith(this,arguments);return this},isResolved:function(){return!!(d||b)},cancel:function(){e=
1;a=[];return this}};return f},Deferred:function(a){var b=c._Deferred(),d=c._Deferred(),e;c.extend(b,{then:function(f,g){b.done(f).fail(g);return this},always:function(){return b.done.apply(b,arguments).fail.apply(this,arguments)},fail:d.done,rejectWith:d.resolveWith,reject:d.resolve,isRejected:d.isResolved,pipe:function(f,g){return c.Deferred(function(i){c.each({done:[f,"resolve"],fail:[g,"reject"]},function(l,m){var n=m[0],r=m[1],A;c.isFunction(n)?b[l](function(){(A=n.apply(this,arguments))&&c.isFunction(A.promise)?
A.promise().then(i.resolve,i.reject):i[r](A)}):b[l](i[r])})}).promise()},promise:function(f){if(f==null){if(e)return e;e=f={}}for(var g=pa.length;g--;)f[pa[g]]=b[pa[g]];return f}});b.done(d.cancel).fail(b.cancel);delete b.cancel;a&&a.call(b,b);return b},when:function(a){function b(l){return function(m){d[l]=arguments.length>1?Ma.call(arguments,0):m;--g||i.resolveWith(i,Ma.call(d,0))}}var d=arguments,e=0,f=d.length,g=f,i=f<=1&&a&&c.isFunction(a.promise)?a:c.Deferred();if(f>1){for(;e<f;e++)if(d[e]&&
c.isFunction(d[e].promise))d[e].promise().then(b(e),i.reject);else--g;g||i.resolveWith(i,d)}else if(i!==a)i.resolveWith(i,f?[a]:[]);return i.promise()}});c.support=function(){var a=x.createElement("div"),b=x.documentElement,d,e,f,g,i,l;a.setAttribute("className","t");a.innerHTML="   <link/><table></table><a href='/a' style='top:1px;float:left;opacity:.55;'>a</a><input type='checkbox'/>";d=a.getElementsByTagName("*");e=a.getElementsByTagName("a")[0];if(!d||!d.length||!e)return{};f=x.createElement("select");
g=f.appendChild(x.createElement("option"));d=a.getElementsByTagName("input")[0];i={leadingWhitespace:a.firstChild.nodeType===3,tbody:!a.getElementsByTagName("tbody").length,htmlSerialize:!!a.getElementsByTagName("link").length,style:/top/.test(e.getAttribute("style")),hrefNormalized:e.getAttribute("href")==="/a",opacity:/^0.55$/.test(e.style.opacity),cssFloat:!!e.style.cssFloat,checkOn:d.value==="on",optSelected:g.selected,getSetAttribute:a.className!=="t",submitBubbles:true,changeBubbles:true,focusinBubbles:false,
deleteExpando:true,noCloneEvent:true,inlineBlockNeedsLayout:false,shrinkWrapBlocks:false,reliableMarginRight:true};d.checked=true;i.noCloneChecked=d.cloneNode(true).checked;f.disabled=true;i.optDisabled=!g.disabled;try{delete a.test}catch(m){i.deleteExpando=false}if(!a.addEventListener&&a.attachEvent&&a.fireEvent){a.attachEvent("onclick",function n(){i.noCloneEvent=false;a.detachEvent("onclick",n)});a.cloneNode(true).fireEvent("onclick")}d=x.createElement("input");d.value="t";d.setAttribute("type",
"radio");i.radioValue=d.value==="t";d.setAttribute("checked","checked");a.appendChild(d);e=x.createDocumentFragment();e.appendChild(a.firstChild);i.checkClone=e.cloneNode(true).cloneNode(true).lastChild.checked;a.innerHTML="";a.style.width=a.style.paddingLeft="1px";e=x.createElement("body");f={visibility:"hidden",width:0,height:0,border:0,margin:0,background:"none"};for(l in f)e.style[l]=f[l];e.appendChild(a);b.insertBefore(e,b.firstChild);i.appendChecked=d.checked;i.boxModel=a.offsetWidth===2;if("zoom"in
a.style){a.style.display="inline";a.style.zoom=1;i.inlineBlockNeedsLayout=a.offsetWidth===2;a.style.display="";a.innerHTML="<div style='width:4px;'></div>";i.shrinkWrapBlocks=a.offsetWidth!==2}a.innerHTML="<table><tr><td style='padding:0;border:0;display:none'></td><td>t</td></tr></table>";f=a.getElementsByTagName("td");d=f[0].offsetHeight===0;f[0].style.display="";f[1].style.display="none";i.reliableHiddenOffsets=d&&f[0].offsetHeight===0;a.innerHTML="";if(x.defaultView&&x.defaultView.getComputedStyle){d=
x.createElement("div");d.style.width="0";d.style.marginRight="0";a.appendChild(d);i.reliableMarginRight=(parseInt((x.defaultView.getComputedStyle(d,null)||{marginRight:0}).marginRight,10)||0)===0}e.innerHTML="";b.removeChild(e);if(a.attachEvent)for(l in{submit:1,change:1,focusin:1}){b="on"+l;d=b in a;if(!d){a.setAttribute(b,"return;");d=typeof a[b]==="function"}i[l+"Bubbles"]=d}return i}();c.boxModel=c.support.boxModel;var hb=/^(?:\{.*\}|\[.*\])$/,gb=/([a-z])([A-Z])/g;c.extend({cache:{},uuid:0,expando:"jQuery"+
(c.fn.jquery+Math.random()).replace(/\D/g,""),noData:{embed:true,object:"clsid:D27CDB6E-AE6D-11cf-96B8-444553540000",applet:true},hasData:function(a){a=a.nodeType?c.cache[a[c.expando]]:a[c.expando];return!!a&&!ia(a)},data:function(a,b,d,e){if(c.acceptData(a)){var f=c.expando,g=typeof b==="string",i=a.nodeType,l=i?c.cache:a,m=i?a[c.expando]:a[c.expando]&&c.expando;if(!((!m||e&&m&&!l[m][f])&&g&&d===v)){if(!m)if(i)a[c.expando]=m=++c.uuid;else m=c.expando;if(!l[m]){l[m]={};if(!i)l[m].toJSON=c.noop}if(typeof b===
"object"||typeof b==="function")if(e)l[m][f]=c.extend(l[m][f],b);else l[m]=c.extend(l[m],b);a=l[m];if(e){a[f]||(a[f]={});a=a[f]}if(d!==v)a[c.camelCase(b)]=d;if(b==="events"&&!a[b])return a[f]&&a[f].events;return g?a[c.camelCase(b)]:a}}},removeData:function(a,b,d){if(c.acceptData(a)){var e=c.expando,f=a.nodeType,g=f?c.cache:a,i=f?a[c.expando]:c.expando;if(g[i]){if(b){var l=d?g[i][e]:g[i];if(l){delete l[b];if(!ia(l))return}}if(d){delete g[i][e];if(!ia(g[i]))return}b=g[i][e];if(c.support.deleteExpando||
g!=E)delete g[i];else g[i]=null;if(b){g[i]={};if(!f)g[i].toJSON=c.noop;g[i][e]=b}else if(f)if(c.support.deleteExpando)delete a[c.expando];else if(a.removeAttribute)a.removeAttribute(c.expando);else a[c.expando]=null}}},_data:function(a,b,d){return c.data(a,b,d,true)},acceptData:function(a){if(a.nodeName){var b=c.noData[a.nodeName.toLowerCase()];if(b)return!(b===true||a.getAttribute("classid")!==b)}return true}});c.fn.extend({data:function(a,b){var d=null;if(typeof a==="undefined"){if(this.length){d=
c.data(this[0]);if(this[0].nodeType===1)for(var e=this[0].attributes,f,g=0,i=e.length;g<i;g++){f=e[g].name;if(f.indexOf("data-")===0){f=c.camelCase(f.substring(5));wa(this[0],f,d[f])}}}return d}else if(typeof a==="object")return this.each(function(){c.data(this,a)});var l=a.split(".");l[1]=l[1]?"."+l[1]:"";if(b===v){d=this.triggerHandler("getData"+l[1]+"!",[l[0]]);if(d===v&&this.length){d=c.data(this[0],a);d=wa(this[0],a,d)}return d===v&&l[1]?this.data(l[0]):d}else return this.each(function(){var m=
c(this),n=[l[0],b];m.triggerHandler("setData"+l[1]+"!",n);c.data(this,a,b);m.triggerHandler("changeData"+l[1]+"!",n)})},removeData:function(a){return this.each(function(){c.removeData(this,a)})}});c.extend({_mark:function(a,b){if(a){b=(b||"fx")+"mark";c.data(a,b,(c.data(a,b,v,true)||0)+1,true)}},_unmark:function(a,b,d){if(a!==true){d=b;b=a;a=false}if(b){d=d||"fx";var e=d+"mark";if(a=a?0:(c.data(b,e,v,true)||1)-1)c.data(b,e,a,true);else{c.removeData(b,e,true);xa(b,d,"mark")}}},queue:function(a,b,d){if(a){b=
(b||"fx")+"queue";var e=c.data(a,b,v,true);if(d)if(!e||c.isArray(d))e=c.data(a,b,c.makeArray(d),true);else e.push(d);return e||[]}},dequeue:function(a,b){b=b||"fx";var d=c.queue(a,b),e=d.shift();if(e==="inprogress")e=d.shift();if(e){b==="fx"&&d.unshift("inprogress");e.call(a,function(){c.dequeue(a,b)})}if(!d.length){c.removeData(a,b+"queue",true);xa(a,b,"queue")}}});c.fn.extend({queue:function(a,b){if(typeof a!=="string"){b=a;a="fx"}if(b===v)return c.queue(this[0],a);return this.each(function(){var d=
c.queue(this,a,b);a==="fx"&&d[0]!=="inprogress"&&c.dequeue(this,a)})},dequeue:function(a){return this.each(function(){c.dequeue(this,a)})},delay:function(a,b){a=c.fx?c.fx.speeds[a]||a:a;b=b||"fx";return this.queue(b,function(){var d=this;setTimeout(function(){c.dequeue(d,b)},a)})},clearQueue:function(a){return this.queue(a||"fx",[])},promise:function(a,b){function d(){--g||e.resolveWith(f,[f])}if(typeof a!=="string"){b=a;a=v}a=a||"fx";var e=c.Deferred(),f=this;b=f.length;var g=1,i=a+"defer",l=a+"queue";
a=a+"mark";for(var m;b--;)if(m=c.data(f[b],i,v,true)||(c.data(f[b],l,v,true)||c.data(f[b],a,v,true))&&c.data(f[b],i,c._Deferred(),true)){g++;m.done(d)}d();return e.promise()}});var Na=/[\n\t\r]/g,qa=/\s+/,yb=/\r/g,zb=/^(?:button|input)$/i,Ab=/^(?:button|input|object|select|textarea)$/i,Bb=/^a(?:rea)?$/i,Oa=/^(?:autofocus|autoplay|async|checked|controls|defer|disabled|hidden|loop|multiple|open|readonly|required|scoped|selected)$/i,Cb=/\:/,S,Pa;c.fn.extend({attr:function(a,b){return c.access(this,a,
b,true,c.attr)},removeAttr:function(a){return this.each(function(){c.removeAttr(this,a)})},prop:function(a,b){return c.access(this,a,b,true,c.prop)},removeProp:function(a){a=c.propFix[a]||a;return this.each(function(){try{this[a]=v;delete this[a]}catch(b){}})},addClass:function(a){if(c.isFunction(a))return this.each(function(n){var r=c(this);r.addClass(a.call(this,n,r.attr("class")||""))});if(a&&typeof a==="string")for(var b=(a||"").split(qa),d=0,e=this.length;d<e;d++){var f=this[d];if(f.nodeType===
1)if(f.className){for(var g=" "+f.className+" ",i=f.className,l=0,m=b.length;l<m;l++)if(g.indexOf(" "+b[l]+" ")<0)i+=" "+b[l];f.className=c.trim(i)}else f.className=a}return this},removeClass:function(a){if(c.isFunction(a))return this.each(function(m){var n=c(this);n.removeClass(a.call(this,m,n.attr("class")))});if(a&&typeof a==="string"||a===v)for(var b=(a||"").split(qa),d=0,e=this.length;d<e;d++){var f=this[d];if(f.nodeType===1&&f.className)if(a){for(var g=(" "+f.className+" ").replace(Na," "),
i=0,l=b.length;i<l;i++)g=g.replace(" "+b[i]+" "," ");f.className=c.trim(g)}else f.className=""}return this},toggleClass:function(a,b){var d=typeof a,e=typeof b==="boolean";if(c.isFunction(a))return this.each(function(f){var g=c(this);g.toggleClass(a.call(this,f,g.attr("class"),b),b)});return this.each(function(){if(d==="string")for(var f,g=0,i=c(this),l=b,m=a.split(qa);f=m[g++];){l=e?l:!i.hasClass(f);i[l?"addClass":"removeClass"](f)}else if(d==="undefined"||d==="boolean"){this.className&&c._data(this,
"__className__",this.className);this.className=this.className||a===false?"":c._data(this,"__className__")||""}})},hasClass:function(a){a=" "+a+" ";for(var b=0,d=this.length;b<d;b++)if((" "+this[b].className+" ").replace(Na," ").indexOf(a)>-1)return true;return false},val:function(a){var b,d,e=this[0];if(!arguments.length){if(e){if((b=c.valHooks[e.nodeName.toLowerCase()]||c.valHooks[e.type])&&"get"in b&&(d=b.get(e,"value"))!==v)return d;return(e.value||"").replace(yb,"")}return v}var f=c.isFunction(a);
return this.each(function(g){var i=c(this);if(this.nodeType===1){g=f?a.call(this,g,i.val()):a;if(g==null)g="";else if(typeof g==="number")g+="";else if(c.isArray(g))g=c.map(g,function(l){return l==null?"":l+""});b=c.valHooks[this.nodeName.toLowerCase()]||c.valHooks[this.type];if(!b||!("set"in b)||b.set(this,g,"value")===v)this.value=g}})}});c.extend({valHooks:{option:{get:function(a){var b=a.attributes.value;return!b||b.specified?a.value:a.text}},select:{get:function(a){var b,d=a.selectedIndex,e=
[],f=a.options;a=a.type==="select-one";if(d<0)return null;for(var g=a?d:0,i=a?d+1:f.length;g<i;g++){b=f[g];if(b.selected&&(c.support.optDisabled?!b.disabled:b.getAttribute("disabled")===null)&&(!b.parentNode.disabled||!c.nodeName(b.parentNode,"optgroup"))){b=c(b).val();if(a)return b;e.push(b)}}if(a&&!e.length&&f.length)return c(f[d]).val();return e},set:function(a,b){var d=c.makeArray(b);c(a).find("option").each(function(){this.selected=c.inArray(c(this).val(),d)>=0});if(!d.length)a.selectedIndex=
-1;return d}}},attrFn:{val:true,css:true,html:true,text:true,data:true,width:true,height:true,offset:true},attrFix:{tabindex:"tabIndex"},attr:function(a,b,d,e){var f=a.nodeType;if(!a||f===3||f===8||f===2)return v;if(e&&b in c.attrFn)return c(a)[b](d);if(!("getAttribute"in a))return c.prop(a,b,d);var g;b=(f=f!==1||!c.isXMLDoc(a))&&c.attrFix[b]||b;e=c.attrHooks[b];if(!e)if(Oa.test(b)&&(typeof d==="boolean"||d===v||d.toLowerCase()===b.toLowerCase()))e=Pa;else if(S&&(c.nodeName(a,"form")||Cb.test(b)))e=
S;if(d!==v)if(d===null){c.removeAttr(a,b);return v}else if(e&&"set"in e&&f&&(g=e.set(a,d,b))!==v)return g;else{a.setAttribute(b,""+d);return d}else if(e&&"get"in e&&f)return e.get(a,b);else{g=a.getAttribute(b);return g===null?v:g}},removeAttr:function(a,b){var d;if(a.nodeType===1){b=c.attrFix[b]||b;if(c.support.getSetAttribute)a.removeAttribute(b);else{c.attr(a,b,"");a.removeAttributeNode(a.getAttributeNode(b))}if(Oa.test(b)&&(d=c.propFix[b]||b)in a)a[d]=false}},attrHooks:{type:{set:function(a,b){if(zb.test(a.nodeName)&&
a.parentNode)c.error("type property can't be changed");else if(!c.support.radioValue&&b==="radio"&&c.nodeName(a,"input")){var d=a.value;a.setAttribute("type",b);if(d)a.value=d;return b}}},tabIndex:{get:function(a){var b=a.getAttributeNode("tabIndex");return b&&b.specified?parseInt(b.value,10):Ab.test(a.nodeName)||Bb.test(a.nodeName)&&a.href?0:v}}},propFix:{tabindex:"tabIndex",readonly:"readOnly","for":"htmlFor","class":"className",maxlength:"maxLength",cellspacing:"cellSpacing",cellpadding:"cellPadding",
rowspan:"rowSpan",colspan:"colSpan",usemap:"useMap",frameborder:"frameBorder",contenteditable:"contentEditable"},prop:function(a,b,d){var e=a.nodeType;if(!a||e===3||e===8||e===2)return v;var f;b=(e!==1||!c.isXMLDoc(a))&&c.propFix[b]||b;e=c.propHooks[b];return d!==v?e&&"set"in e&&(f=e.set(a,d,b))!==v?f:(a[b]=d):e&&"get"in e&&(f=e.get(a,b))!==v?f:a[b]},propHooks:{}});Pa={get:function(a,b){return a[c.propFix[b]||b]?b.toLowerCase():v},set:function(a,b,d){var e;if(b===false)c.removeAttr(a,d);else{e=c.propFix[d]||
d;if(e in a)a[e]=b;a.setAttribute(d,d.toLowerCase())}return d}};c.attrHooks.value={get:function(a,b){if(S&&c.nodeName(a,"button"))return S.get(a,b);return a.value},set:function(a,b,d){if(S&&c.nodeName(a,"button"))return S.set(a,b,d);a.value=b}};if(!c.support.getSetAttribute){c.attrFix=c.propFix;S=c.attrHooks.name=c.valHooks.button={get:function(a,b){return(a=a.getAttributeNode(b))&&a.nodeValue!==""?a.nodeValue:v},set:function(a,b,d){if(a=a.getAttributeNode(d))return a.nodeValue=b}};c.each(["width",
"height"],function(a,b){c.attrHooks[b]=c.extend(c.attrHooks[b],{set:function(d,e){if(e===""){d.setAttribute(b,"auto");return e}}})})}c.support.hrefNormalized||c.each(["href","src","width","height"],function(a,b){c.attrHooks[b]=c.extend(c.attrHooks[b],{get:function(d){d=d.getAttribute(b,2);return d===null?v:d}})});if(!c.support.style)c.attrHooks.style={get:function(a){return a.style.cssText.toLowerCase()||v},set:function(a,b){return a.style.cssText=""+b}};if(!c.support.optSelected)c.propHooks.selected=
c.extend(c.propHooks.selected,{get:function(){}});c.support.checkOn||c.each(["radio","checkbox"],function(){c.valHooks[this]={get:function(a){return a.getAttribute("value")===null?"on":a.value}}});c.each(["radio","checkbox"],function(){c.valHooks[this]=c.extend(c.valHooks[this],{set:function(a,b){if(c.isArray(b))return a.checked=c.inArray(c(a).val(),b)>=0}})});var ja=/\.(.*)$/,ra=/^(?:textarea|input|select)$/i,jb=/\./g,kb=/ /g,Db=/[^\w\s.|`]/g,Eb=function(a){return a.replace(Db,"\\$&")};c.event={add:function(a,
b,d,e){if(!(a.nodeType===3||a.nodeType===8)){if(d===false)d=Q;else if(!d)return;var f,g;if(d.handler){f=d;d=f.handler}if(!d.guid)d.guid=c.guid++;if(g=c._data(a)){var i=g.events,l=g.handle;if(!i)g.events=i={};if(!l)g.handle=l=function(C){return typeof c!=="undefined"&&(!C||c.event.triggered!==C.type)?c.event.handle.apply(l.elem,arguments):v};l.elem=a;b=b.split(" ");for(var m,n=0,r;m=b[n++];){g=f?c.extend({},f):{handler:d,data:e};if(m.indexOf(".")>-1){r=m.split(".");m=r.shift();g.namespace=r.slice(0).sort().join(".")}else{r=
[];g.namespace=""}g.type=m;if(!g.guid)g.guid=d.guid;var A=i[m],B=c.event.special[m]||{};if(!A){A=i[m]=[];if(!B.setup||B.setup.call(a,e,r,l)===false)if(a.addEventListener)a.addEventListener(m,l,false);else a.attachEvent&&a.attachEvent("on"+m,l)}if(B.add){B.add.call(a,g);if(!g.handler.guid)g.handler.guid=d.guid}A.push(g);c.event.global[m]=true}a=null}}},global:{},remove:function(a,b,d,e){if(!(a.nodeType===3||a.nodeType===8)){if(d===false)d=Q;var f,g,i=0,l,m,n,r,A,B,C=c.hasData(a)&&c._data(a),G=C&&C.events;
if(C&&G){if(b&&b.type){d=b.handler;b=b.type}if(!b||typeof b==="string"&&b.charAt(0)==="."){b=b||"";for(f in G)c.event.remove(a,f+b)}else{for(b=b.split(" ");f=b[i++];){r=f;l=f.indexOf(".")<0;m=[];if(!l){m=f.split(".");f=m.shift();n=new RegExp("(^|\\.)"+c.map(m.slice(0).sort(),Eb).join("\\.(?:.*\\.)?")+"(\\.|$)")}if(A=G[f])if(d){r=c.event.special[f]||{};for(g=e||0;g<A.length;g++){B=A[g];if(d.guid===B.guid){if(l||n.test(B.namespace)){e==null&&A.splice(g--,1);r.remove&&r.remove.call(a,B)}if(e!=null)break}}if(A.length===
0||e!=null&&A.length===1){if(!r.teardown||r.teardown.call(a,m)===false)c.removeEvent(a,f,C.handle);delete G[f]}}else for(g=0;g<A.length;g++){B=A[g];if(l||n.test(B.namespace)){c.event.remove(a,r,B.handler,g);A.splice(g--,1)}}}if(c.isEmptyObject(G)){if(b=C.handle)b.elem=null;delete C.events;delete C.handle;c.isEmptyObject(C)&&c.removeData(a,v,true)}}}}},customEvent:{getData:true,setData:true,changeData:true},trigger:function(a,b,d,e){var f=a.type||a,g=[],i;if(f.indexOf("!")>=0){f=f.slice(0,-1);i=true}if(f.indexOf(".")>=
0){g=f.split(".");f=g.shift();g.sort()}if(!((!d||c.event.customEvent[f])&&!c.event.global[f])){a=typeof a==="object"?a[c.expando]?a:new c.Event(f,a):new c.Event(f);a.type=f;a.exclusive=i;a.namespace=g.join(".");a.namespace_re=new RegExp("(^|\\.)"+g.join("\\.(?:.*\\.)?")+"(\\.|$)");if(e||!d){a.preventDefault();a.stopPropagation()}if(d){if(!(d.nodeType===3||d.nodeType===8)){a.result=v;a.target=d;b=b?c.makeArray(b):[];b.unshift(a);g=d;e=f.indexOf(":")<0?"on"+f:"";do{i=c._data(g,"handle");a.currentTarget=
g;i&&i.apply(g,b);if(e&&c.acceptData(g)&&g[e]&&g[e].apply(g,b)===false){a.result=false;a.preventDefault()}g=g.parentNode||g.ownerDocument||g===a.target.ownerDocument&&E}while(g&&!a.isPropagationStopped());if(!a.isDefaultPrevented()){var l;g=c.event.special[f]||{};if((!g._default||g._default.call(d.ownerDocument,a)===false)&&!(f==="click"&&c.nodeName(d,"a"))&&c.acceptData(d)){try{if(e&&d[f]){if(l=d[e])d[e]=null;c.event.triggered=f;d[f]()}}catch(m){}if(l)d[e]=l;c.event.triggered=v}}return a.result}}else c.each(c.cache,
function(){var n=this[c.expando];n&&n.events&&n.events[f]&&c.event.trigger(a,b,n.handle.elem)})}},handle:function(a){a=c.event.fix(a||E.event);var b=((c._data(this,"events")||{})[a.type]||[]).slice(0),d=!a.exclusive&&!a.namespace,e=Array.prototype.slice.call(arguments,0);e[0]=a;a.currentTarget=this;for(var f=0,g=b.length;f<g;f++){var i=b[f];if(d||a.namespace_re.test(i.namespace)){a.handler=i.handler;a.data=i.data;a.handleObj=i;i=i.handler.apply(this,e);if(i!==v){a.result=i;if(i===false){a.preventDefault();
a.stopPropagation()}}if(a.isImmediatePropagationStopped())break}}return a.result},props:"altKey attrChange attrName bubbles button cancelable charCode clientX clientY ctrlKey currentTarget data detail eventPhase fromElement handler keyCode layerX layerY metaKey newValue offsetX offsetY pageX pageY prevValue relatedNode relatedTarget screenX screenY shiftKey srcElement target toElement view wheelDelta which".split(" "),fix:function(a){if(a[c.expando])return a;var b=a;a=c.Event(b);for(var d=this.props.length,
e;d;){e=this.props[--d];a[e]=b[e]}if(!a.target)a.target=a.srcElement||x;if(a.target.nodeType===3)a.target=a.target.parentNode;if(!a.relatedTarget&&a.fromElement)a.relatedTarget=a.fromElement===a.target?a.toElement:a.fromElement;if(a.pageX==null&&a.clientX!=null){d=a.target.ownerDocument||x;b=d.documentElement;d=d.body;a.pageX=a.clientX+(b&&b.scrollLeft||d&&d.scrollLeft||0)-(b&&b.clientLeft||d&&d.clientLeft||0);a.pageY=a.clientY+(b&&b.scrollTop||d&&d.scrollTop||0)-(b&&b.clientTop||d&&d.clientTop||
0)}if(a.which==null&&(a.charCode!=null||a.keyCode!=null))a.which=a.charCode!=null?a.charCode:a.keyCode;if(!a.metaKey&&a.ctrlKey)a.metaKey=a.ctrlKey;if(!a.which&&a.button!==v)a.which=a.button&1?1:a.button&2?3:a.button&4?2:0;return a},guid:1E8,proxy:c.proxy,special:{ready:{setup:c.bindReady,teardown:c.noop},live:{add:function(a){c.event.add(this,ba(a.origType,a.selector),c.extend({},a,{handler:ib,guid:a.handler.guid}))},remove:function(a){c.event.remove(this,ba(a.origType,a.selector),a)}},beforeunload:{setup:function(a,
b,d){if(c.isWindow(this))this.onbeforeunload=d},teardown:function(a,b){if(this.onbeforeunload===b)this.onbeforeunload=null}}}};c.removeEvent=x.removeEventListener?function(a,b,d){a.removeEventListener&&a.removeEventListener(b,d,false)}:function(a,b,d){a.detachEvent&&a.detachEvent("on"+b,d)};c.Event=function(a,b){if(!this.preventDefault)return new c.Event(a,b);if(a&&a.type){this.originalEvent=a;this.type=a.type;this.isDefaultPrevented=a.defaultPrevented||a.returnValue===false||a.getPreventDefault&&
a.getPreventDefault()?aa:Q}else this.type=a;b&&c.extend(this,b);this.timeStamp=c.now();this[c.expando]=true};c.Event.prototype={preventDefault:function(){this.isDefaultPrevented=aa;var a=this.originalEvent;if(a)if(a.preventDefault)a.preventDefault();else a.returnValue=false},stopPropagation:function(){this.isPropagationStopped=aa;var a=this.originalEvent;if(a){a.stopPropagation&&a.stopPropagation();a.cancelBubble=true}},stopImmediatePropagation:function(){this.isImmediatePropagationStopped=aa;this.stopPropagation()},
isDefaultPrevented:Q,isPropagationStopped:Q,isImmediatePropagationStopped:Q};var Qa=function(a){var b=a.relatedTarget;a.type=a.data;try{if(!(b&&b!==x&&!b.parentNode)){for(;b&&b!==this;)b=b.parentNode;b!==this&&c.event.handle.apply(this,arguments)}}catch(d){}},Ra=function(a){a.type=a.data;c.event.handle.apply(this,arguments)};c.each({mouseenter:"mouseover",mouseleave:"mouseout"},function(a,b){c.event.special[a]={setup:function(d){c.event.add(this,b,d&&d.selector?Ra:Qa,a)},teardown:function(d){c.event.remove(this,
b,d&&d.selector?Ra:Qa)}}});if(!c.support.submitBubbles)c.event.special.submit={setup:function(){if(c.nodeName(this,"form"))return false;else{c.event.add(this,"click.specialSubmit",function(a){var b=a.target,d=b.type;if((d==="submit"||d==="image")&&c(b).closest("form").length)ya("submit",this,arguments)});c.event.add(this,"keypress.specialSubmit",function(a){var b=a.target,d=b.type;if((d==="text"||d==="password")&&c(b).closest("form").length&&a.keyCode===13)ya("submit",this,arguments)})}},teardown:function(){c.event.remove(this,
".specialSubmit")}};if(!c.support.changeBubbles){var $,Sa=function(a){var b=a.type,d=a.value;if(b==="radio"||b==="checkbox")d=a.checked;else if(b==="select-multiple")d=a.selectedIndex>-1?c.map(a.options,function(e){return e.selected}).join("-"):"";else if(c.nodeName(a,"select"))d=a.selectedIndex;return d},fa=function(a,b){var d=a.target,e,f;if(!(!ra.test(d.nodeName)||d.readOnly)){e=c._data(d,"_change_data");f=Sa(d);if(a.type!=="focusout"||d.type!=="radio")c._data(d,"_change_data",f);if(!(e===v||f===
e))if(e!=null||f){a.type="change";a.liveFired=v;c.event.trigger(a,b,d)}}};c.event.special.change={filters:{focusout:fa,beforedeactivate:fa,click:function(a){var b=a.target,d=c.nodeName(b,"input")?b.type:"";if(d==="radio"||d==="checkbox"||c.nodeName(b,"select"))fa.call(this,a)},keydown:function(a){var b=a.target,d=c.nodeName(b,"input")?b.type:"";if(a.keyCode===13&&!c.nodeName(b,"textarea")||a.keyCode===32&&(d==="checkbox"||d==="radio")||d==="select-multiple")fa.call(this,a)},beforeactivate:function(a){a=
a.target;c._data(a,"_change_data",Sa(a))}},setup:function(){if(this.type==="file")return false;for(var a in $)c.event.add(this,a+".specialChange",$[a]);return ra.test(this.nodeName)},teardown:function(){c.event.remove(this,".specialChange");return ra.test(this.nodeName)}};$=c.event.special.change.filters;$.focus=$.beforeactivate}c.support.focusinBubbles||c.each({focus:"focusin",blur:"focusout"},function(a,b){function d(f){var g=c.event.fix(f);g.type=b;g.originalEvent={};c.event.trigger(g,null,g.target);
g.isDefaultPrevented()&&f.preventDefault()}var e=0;c.event.special[b]={setup:function(){e++===0&&x.addEventListener(a,d,true)},teardown:function(){--e===0&&x.removeEventListener(a,d,true)}}});c.each(["bind","one"],function(a,b){c.fn[b]=function(d,e,f){var g;if(typeof d==="object"){for(var i in d)this[b](i,e,d[i],f);return this}if(arguments.length===2||e===false){f=e;e=v}if(b==="one"){g=function(m){c(this).unbind(m,g);return f.apply(this,arguments)};g.guid=f.guid||c.guid++}else g=f;if(d==="unload"&&
b!=="one")this.one(d,e,f);else{i=0;for(var l=this.length;i<l;i++)c.event.add(this[i],d,g,e)}return this}});c.fn.extend({unbind:function(a,b){if(typeof a==="object"&&!a.preventDefault)for(var d in a)this.unbind(d,a[d]);else{d=0;for(var e=this.length;d<e;d++)c.event.remove(this[d],a,b)}return this},delegate:function(a,b,d,e){return this.live(b,d,e,a)},undelegate:function(a,b,d){return arguments.length===0?this.unbind("live"):this.die(b,null,d,a)},trigger:function(a,b){return this.each(function(){c.event.trigger(a,
b,this)})},triggerHandler:function(a,b){if(this[0])return c.event.trigger(a,b,this[0],true)},toggle:function(a){var b=arguments,d=a.guid||c.guid++,e=0,f=function(g){var i=(c.data(this,"lastToggle"+a.guid)||0)%e;c.data(this,"lastToggle"+a.guid,i+1);g.preventDefault();return b[i].apply(this,arguments)||false};for(f.guid=d;e<b.length;)b[e++].guid=d;return this.click(f)},hover:function(a,b){return this.mouseenter(a).mouseleave(b||a)}});var sa={focus:"focusin",blur:"focusout",mouseenter:"mouseover",mouseleave:"mouseout"};
c.each(["live","die"],function(a,b){c.fn[b]=function(d,e,f,g){var i=0,l,m,n=g||this.selector,r=g?this:c(this.context);if(typeof d==="object"&&!d.preventDefault){for(l in d)r[b](l,e,d[l],n);return this}if(b==="die"&&!d&&g&&g.charAt(0)==="."){r.unbind(g);return this}if(e===false||c.isFunction(e)){f=e||Q;e=v}for(d=(d||"").split(" ");(g=d[i++])!=null;){l=ja.exec(g);m="";if(l){m=l[0];g=g.replace(ja,"")}if(g==="hover")d.push("mouseenter"+m,"mouseleave"+m);else{l=g;if(sa[g]){d.push(sa[g]+m);g+=m}else g=
(sa[g]||g)+m;if(b==="live"){m=0;for(var A=r.length;m<A;m++)c.event.add(r[m],"live."+ba(g,n),{data:e,selector:n,handler:f,origType:g,origHandler:f,preType:l})}else r.unbind("live."+ba(g,n),f)}}return this}});c.each("blur focus focusin focusout load resize scroll unload click dblclick mousedown mouseup mousemove mouseover mouseout mouseenter mouseleave change select submit keydown keypress keyup error".split(" "),function(a,b){c.fn[b]=function(d,e){if(e==null){e=d;d=null}return arguments.length>0?this.bind(b,
d,e):this.trigger(b)};if(c.attrFn)c.attrFn[b]=true});(function(){function a(h,j,o,p,q,t){q=0;for(var y=p.length;q<y;q++){var w=p[q];if(w){var D=false;for(w=w[h];w;){if(w.sizcache===o){D=p[w.sizset];break}if(w.nodeType===1&&!t){w.sizcache=o;w.sizset=q}if(w.nodeName.toLowerCase()===j){D=w;break}w=w[h]}p[q]=D}}}function b(h,j,o,p,q,t){q=0;for(var y=p.length;q<y;q++){var w=p[q];if(w){var D=false;for(w=w[h];w;){if(w.sizcache===o){D=p[w.sizset];break}if(w.nodeType===1){if(!t){w.sizcache=o;w.sizset=q}if(typeof j!==
"string"){if(w===j){D=true;break}}else if(n.filter(j,[w]).length>0){D=w;break}}w=w[h]}p[q]=D}}}var d=/((?:\((?:\([^()]+\)|[^()]+)+\)|\[(?:\[[^\[\]]*\]|['"][^'"]*['"]|[^\[\]'"]+)+\]|\\.|[^ >+~,(\[\\]+)+|[>+~])(\s*,\s*)?((?:.|\r|\n)*)/g,e=0,f=Object.prototype.toString,g=false,i=true,l=/\\/g,m=/\W/;[0,0].sort(function(){i=false;return 0});var n=function(h,j,o,p){o=o||[];var q=j=j||x;if(j.nodeType!==1&&j.nodeType!==9)return[];if(!h||typeof h!=="string")return o;var t,y,w,D,L,k=true,s=n.isXML(j),u=[],
z=h;do{d.exec("");if(t=d.exec(z)){z=t[3];u.push(t[1]);if(t[2]){D=t[3];break}}}while(t);if(u.length>1&&A.exec(h))if(u.length===2&&r.relative[u[0]])y=O(u[0]+u[1],j);else for(y=r.relative[u[0]]?[j]:n(u.shift(),j);u.length;){h=u.shift();if(r.relative[h])h+=u.shift();y=O(h,y)}else{if(!p&&u.length>1&&j.nodeType===9&&!s&&r.match.ID.test(u[0])&&!r.match.ID.test(u[u.length-1])){t=n.find(u.shift(),j,s);j=t.expr?n.filter(t.expr,t.set)[0]:t.set[0]}if(j){t=p?{expr:u.pop(),set:G(p)}:n.find(u.pop(),u.length===1&&
(u[0]==="~"||u[0]==="+")&&j.parentNode?j.parentNode:j,s);y=t.expr?n.filter(t.expr,t.set):t.set;if(u.length>0)w=G(y);else k=false;for(;u.length;){t=L=u.pop();if(r.relative[L])t=u.pop();else L="";if(t==null)t=j;r.relative[L](w,t,s)}}else w=[]}w||(w=y);w||n.error(L||h);if(f.call(w)==="[object Array]")if(k)if(j&&j.nodeType===1)for(h=0;w[h]!=null;h++){if(w[h]&&(w[h]===true||w[h].nodeType===1&&n.contains(j,w[h])))o.push(y[h])}else for(h=0;w[h]!=null;h++)w[h]&&w[h].nodeType===1&&o.push(y[h]);else o.push.apply(o,
w);else G(w,o);if(D){n(D,q,o,p);n.uniqueSort(o)}return o};n.uniqueSort=function(h){if(I){g=i;h.sort(I);if(g)for(var j=1;j<h.length;j++)h[j]===h[j-1]&&h.splice(j--,1)}return h};n.matches=function(h,j){return n(h,null,null,j)};n.matchesSelector=function(h,j){return n(j,null,null,[h]).length>0};n.find=function(h,j,o){var p;if(!h)return[];for(var q=0,t=r.order.length;q<t;q++){var y,w=r.order[q];if(y=r.leftMatch[w].exec(h)){var D=y[1];y.splice(1,1);if(D.substr(D.length-1)!=="\\"){y[1]=(y[1]||"").replace(l,
"");p=r.find[w](y,j,o);if(p!=null){h=h.replace(r.match[w],"");break}}}}p||(p=typeof j.getElementsByTagName!=="undefined"?j.getElementsByTagName("*"):[]);return{set:p,expr:h}};n.filter=function(h,j,o,p){for(var q,t,y=h,w=[],D=j,L=j&&j[0]&&n.isXML(j[0]);h&&j.length;){for(var k in r.filter)if((q=r.leftMatch[k].exec(h))!=null&&q[2]){var s,u,z=r.filter[k];u=q[1];t=false;q.splice(1,1);if(u.substr(u.length-1)!=="\\"){if(D===w)w=[];if(r.preFilter[k])if(q=r.preFilter[k](q,D,o,w,p,L)){if(q===true)continue}else t=
s=true;if(q)for(var H=0;(u=D[H])!=null;H++)if(u){s=z(u,q,H,D);var F=p^!!s;if(o&&s!=null)if(F)t=true;else D[H]=false;else if(F){w.push(u);t=true}}if(s!==v){o||(D=w);h=h.replace(r.match[k],"");if(!t)return[];break}}}if(h===y)if(t==null)n.error(h);else break;y=h}return D};n.error=function(h){throw"Syntax error, unrecognized expression: "+h;};var r=n.selectors={order:["ID","NAME","TAG"],match:{ID:/#((?:[\w\u00c0-\uFFFF\-]|\\.)+)/,CLASS:/\.((?:[\w\u00c0-\uFFFF\-]|\\.)+)/,NAME:/\[name=['"]*((?:[\w\u00c0-\uFFFF\-]|\\.)+)['"]*\]/,
ATTR:/\[\s*((?:[\w\u00c0-\uFFFF\-]|\\.)+)\s*(?:(\S?=)\s*(?:(['"])(.*?)\3|(#?(?:[\w\u00c0-\uFFFF\-]|\\.)*)|)|)\s*\]/,TAG:/^((?:[\w\u00c0-\uFFFF\*\-]|\\.)+)/,CHILD:/:(only|nth|last|first)-child(?:\(\s*(even|odd|(?:[+\-]?\d+|(?:[+\-]?\d*)?n\s*(?:[+\-]\s*\d+)?))\s*\))?/,POS:/:(nth|eq|gt|lt|first|last|even|odd)(?:\((\d*)\))?(?=[^\-]|$)/,PSEUDO:/:((?:[\w\u00c0-\uFFFF\-]|\\.)+)(?:\((['"]?)((?:\([^\)]+\)|[^\(\)]*)+)\2\))?/},leftMatch:{},attrMap:{"class":"className","for":"htmlFor"},attrHandle:{href:function(h){return h.getAttribute("href")},
type:function(h){return h.getAttribute("type")}},relative:{"+":function(h,j){var o=typeof j==="string",p=o&&!m.test(j);o=o&&!p;if(p)j=j.toLowerCase();p=0;for(var q=h.length,t;p<q;p++)if(t=h[p]){for(;(t=t.previousSibling)&&t.nodeType!==1;);h[p]=o||t&&t.nodeName.toLowerCase()===j?t||false:t===j}o&&n.filter(j,h,true)},">":function(h,j){var o,p=typeof j==="string",q=0,t=h.length;if(p&&!m.test(j))for(j=j.toLowerCase();q<t;q++){if(o=h[q]){o=o.parentNode;h[q]=o.nodeName.toLowerCase()===j?o:false}}else{for(;q<
t;q++)if(o=h[q])h[q]=p?o.parentNode:o.parentNode===j;p&&n.filter(j,h,true)}},"":function(h,j,o){var p,q=e++,t=b;if(typeof j==="string"&&!m.test(j)){p=j=j.toLowerCase();t=a}t("parentNode",j,q,h,p,o)},"~":function(h,j,o){var p,q=e++,t=b;if(typeof j==="string"&&!m.test(j)){p=j=j.toLowerCase();t=a}t("previousSibling",j,q,h,p,o)}},find:{ID:function(h,j,o){if(typeof j.getElementById!=="undefined"&&!o)return(h=j.getElementById(h[1]))&&h.parentNode?[h]:[]},NAME:function(h,j){if(typeof j.getElementsByName!==
"undefined"){var o=[];j=j.getElementsByName(h[1]);for(var p=0,q=j.length;p<q;p++)j[p].getAttribute("name")===h[1]&&o.push(j[p]);return o.length===0?null:o}},TAG:function(h,j){if(typeof j.getElementsByTagName!=="undefined")return j.getElementsByTagName(h[1])}},preFilter:{CLASS:function(h,j,o,p,q,t){h=" "+h[1].replace(l,"")+" ";if(t)return h;t=0;for(var y;(y=j[t])!=null;t++)if(y)if(q^(y.className&&(" "+y.className+" ").replace(/[\t\n\r]/g," ").indexOf(h)>=0))o||p.push(y);else if(o)j[t]=false;return false},
ID:function(h){return h[1].replace(l,"")},TAG:function(h){return h[1].replace(l,"").toLowerCase()},CHILD:function(h){if(h[1]==="nth"){h[2]||n.error(h[0]);h[2]=h[2].replace(/^\+|\s*/g,"");var j=/(-?)(\d*)(?:n([+\-]?\d*))?/.exec(h[2]==="even"&&"2n"||h[2]==="odd"&&"2n+1"||!/\D/.test(h[2])&&"0n+"+h[2]||h[2]);h[2]=j[1]+(j[2]||1)-0;h[3]=j[3]-0}else h[2]&&n.error(h[0]);h[0]=e++;return h},ATTR:function(h,j,o,p,q,t){j=h[1]=h[1].replace(l,"");if(!t&&r.attrMap[j])h[1]=r.attrMap[j];h[4]=(h[4]||h[5]||"").replace(l,
"");if(h[2]==="~=")h[4]=" "+h[4]+" ";return h},PSEUDO:function(h,j,o,p,q){if(h[1]==="not")if((d.exec(h[3])||"").length>1||/^\w/.test(h[3]))h[3]=n(h[3],null,null,j);else{h=n.filter(h[3],j,o,true^q);o||p.push.apply(p,h);return false}else if(r.match.POS.test(h[0])||r.match.CHILD.test(h[0]))return true;return h},POS:function(h){h.unshift(true);return h}},filters:{enabled:function(h){return h.disabled===false&&h.type!=="hidden"},disabled:function(h){return h.disabled===true},checked:function(h){return h.checked===
true},selected:function(h){return h.selected===true},parent:function(h){return!!h.firstChild},empty:function(h){return!h.firstChild},has:function(h,j,o){return!!n(o[3],h).length},header:function(h){return/h\d/i.test(h.nodeName)},text:function(h){var j=h.getAttribute("type"),o=h.type;return h.nodeName.toLowerCase()==="input"&&"text"===o&&(j===o||j===null)},radio:function(h){return h.nodeName.toLowerCase()==="input"&&"radio"===h.type},checkbox:function(h){return h.nodeName.toLowerCase()==="input"&&
"checkbox"===h.type},file:function(h){return h.nodeName.toLowerCase()==="input"&&"file"===h.type},password:function(h){return h.nodeName.toLowerCase()==="input"&&"password"===h.type},submit:function(h){var j=h.nodeName.toLowerCase();return(j==="input"||j==="button")&&"submit"===h.type},image:function(h){return h.nodeName.toLowerCase()==="input"&&"image"===h.type},reset:function(h){var j=h.nodeName.toLowerCase();return(j==="input"||j==="button")&&"reset"===h.type},button:function(h){var j=h.nodeName.toLowerCase();
return j==="input"&&"button"===h.type||j==="button"},input:function(h){return/input|select|textarea|button/i.test(h.nodeName)},focus:function(h){return h===h.ownerDocument.activeElement}},setFilters:{first:function(h,j){return j===0},last:function(h,j,o,p){return j===p.length-1},even:function(h,j){return j%2===0},odd:function(h,j){return j%2===1},lt:function(h,j,o){return j<o[3]-0},gt:function(h,j,o){return j>o[3]-0},nth:function(h,j,o){return o[3]-0===j},eq:function(h,j,o){return o[3]-0===j}},filter:{PSEUDO:function(h,
j,o,p){var q=j[1],t=r.filters[q];if(t)return t(h,o,j,p);else if(q==="contains")return(h.textContent||h.innerText||n.getText([h])||"").indexOf(j[3])>=0;else if(q==="not"){j=j[3];o=0;for(p=j.length;o<p;o++)if(j[o]===h)return false;return true}else n.error(q)},CHILD:function(h,j){var o=j[1],p=h;switch(o){case "only":case "first":for(;p=p.previousSibling;)if(p.nodeType===1)return false;if(o==="first")return true;p=h;case "last":for(;p=p.nextSibling;)if(p.nodeType===1)return false;return true;case "nth":o=
j[2];var q=j[3];if(o===1&&q===0)return true;j=j[0];var t=h.parentNode;if(t&&(t.sizcache!==j||!h.nodeIndex)){var y=0;for(p=t.firstChild;p;p=p.nextSibling)if(p.nodeType===1)p.nodeIndex=++y;t.sizcache=j}h=h.nodeIndex-q;return o===0?h===0:h%o===0&&h/o>=0}},ID:function(h,j){return h.nodeType===1&&h.getAttribute("id")===j},TAG:function(h,j){return j==="*"&&h.nodeType===1||h.nodeName.toLowerCase()===j},CLASS:function(h,j){return(" "+(h.className||h.getAttribute("class"))+" ").indexOf(j)>-1},ATTR:function(h,
j){var o=j[1];h=r.attrHandle[o]?r.attrHandle[o](h):h[o]!=null?h[o]:h.getAttribute(o);o=h+"";var p=j[2];j=j[4];return h==null?p==="!=":p==="="?o===j:p==="*="?o.indexOf(j)>=0:p==="~="?(" "+o+" ").indexOf(j)>=0:!j?o&&h!==false:p==="!="?o!==j:p==="^="?o.indexOf(j)===0:p==="$="?o.substr(o.length-j.length)===j:p==="|="?o===j||o.substr(0,j.length+1)===j+"-":false},POS:function(h,j,o,p){var q=r.setFilters[j[2]];if(q)return q(h,o,j,p)}}},A=r.match.POS,B=function(h,j){return"\\"+(j-0+1)};for(var C in r.match){r.match[C]=
new RegExp(r.match[C].source+/(?![^\[]*\])(?![^\(]*\))/.source);r.leftMatch[C]=new RegExp(/(^(?:.|\r|\n)*?)/.source+r.match[C].source.replace(/\\(\d+)/g,B))}var G=function(h,j){h=Array.prototype.slice.call(h,0);if(j){j.push.apply(j,h);return j}return h};try{Array.prototype.slice.call(x.documentElement.childNodes,0)}catch(R){G=function(h,j){var o=0;j=j||[];if(f.call(h)==="[object Array]")Array.prototype.push.apply(j,h);else if(typeof h.length==="number")for(var p=h.length;o<p;o++)j.push(h[o]);else for(;h[o];o++)j.push(h[o]);
return j}}var I,M;if(x.documentElement.compareDocumentPosition)I=function(h,j){if(h===j){g=true;return 0}if(!h.compareDocumentPosition||!j.compareDocumentPosition)return h.compareDocumentPosition?-1:1;return h.compareDocumentPosition(j)&4?-1:1};else{I=function(h,j){if(h===j){g=true;return 0}else if(h.sourceIndex&&j.sourceIndex)return h.sourceIndex-j.sourceIndex;var o,p,q=[],t=[];o=h.parentNode;p=j.parentNode;var y=o;if(o===p)return M(h,j);else if(o){if(!p)return 1}else return-1;for(;y;){q.unshift(y);
y=y.parentNode}for(y=p;y;){t.unshift(y);y=y.parentNode}o=q.length;p=t.length;for(y=0;y<o&&y<p;y++)if(q[y]!==t[y])return M(q[y],t[y]);return y===o?M(h,t[y],-1):M(q[y],j,1)};M=function(h,j,o){if(h===j)return o;for(h=h.nextSibling;h;){if(h===j)return-1;h=h.nextSibling}return 1}}n.getText=function(h){for(var j="",o,p=0;h[p];p++){o=h[p];if(o.nodeType===3||o.nodeType===4)j+=o.nodeValue;else if(o.nodeType!==8)j+=n.getText(o.childNodes)}return j};(function(){var h=x.createElement("div"),j="script"+(new Date).getTime(),
o=x.documentElement;h.innerHTML="<a name='"+j+"'/>";o.insertBefore(h,o.firstChild);if(x.getElementById(j)){r.find.ID=function(p,q,t){if(typeof q.getElementById!=="undefined"&&!t)return(q=q.getElementById(p[1]))?q.id===p[1]||typeof q.getAttributeNode!=="undefined"&&q.getAttributeNode("id").nodeValue===p[1]?[q]:v:[]};r.filter.ID=function(p,q){var t=typeof p.getAttributeNode!=="undefined"&&p.getAttributeNode("id");return p.nodeType===1&&t&&t.nodeValue===q}}o.removeChild(h);o=h=null})();(function(){var h=
x.createElement("div");h.appendChild(x.createComment(""));if(h.getElementsByTagName("*").length>0)r.find.TAG=function(j,o){o=o.getElementsByTagName(j[1]);if(j[1]==="*"){j=[];for(var p=0;o[p];p++)o[p].nodeType===1&&j.push(o[p]);o=j}return o};h.innerHTML="<a href='#'></a>";if(h.firstChild&&typeof h.firstChild.getAttribute!=="undefined"&&h.firstChild.getAttribute("href")!=="#")r.attrHandle.href=function(j){return j.getAttribute("href",2)};h=null})();x.querySelectorAll&&function(){var h=n,j=x.createElement("div");
j.innerHTML="<p class='TEST'></p>";if(!(j.querySelectorAll&&j.querySelectorAll(".TEST").length===0)){n=function(p,q,t,y){q=q||x;if(!y&&!n.isXML(q)){var w=/^(\w+$)|^\.([\w\-]+$)|^#([\w\-]+$)/.exec(p);if(w&&(q.nodeType===1||q.nodeType===9))if(w[1])return G(q.getElementsByTagName(p),t);else if(w[2]&&r.find.CLASS&&q.getElementsByClassName)return G(q.getElementsByClassName(w[2]),t);if(q.nodeType===9){if(p==="body"&&q.body)return G([q.body],t);else if(w&&w[3]){var D=q.getElementById(w[3]);if(D&&D.parentNode){if(D.id===
w[3])return G([D],t)}else return G([],t)}try{return G(q.querySelectorAll(p),t)}catch(L){}}else if(q.nodeType===1&&q.nodeName.toLowerCase()!=="object"){w=q;var k=(D=q.getAttribute("id"))||"__sizzle__",s=q.parentNode,u=/^\s*[+~]/.test(p);if(D)k=k.replace(/'/g,"\\$&");else q.setAttribute("id",k);if(u&&s)q=q.parentNode;try{if(!u||s)return G(q.querySelectorAll("[id='"+k+"'] "+p),t)}catch(z){}finally{D||w.removeAttribute("id")}}}return h(p,q,t,y)};for(var o in h)n[o]=h[o];j=null}}();(function(){var h=x.documentElement,
j=h.matchesSelector||h.mozMatchesSelector||h.webkitMatchesSelector||h.msMatchesSelector;if(j){var o=!j.call(x.createElement("div"),"div"),p=false;try{j.call(x.documentElement,"[test!='']:sizzle")}catch(q){p=true}n.matchesSelector=function(t,y){y=y.replace(/\=\s*([^'"\]]*)\s*\]/g,"='$1']");if(!n.isXML(t))try{if(p||!r.match.PSEUDO.test(y)&&!/!=/.test(y)){var w=j.call(t,y);if(w||!o||t.document&&t.document.nodeType!==11)return w}}catch(D){}return n(y,null,null,[t]).length>0}}})();(function(){var h=x.createElement("div");
h.innerHTML="<div class='test e'></div><div class='test'></div>";if(!(!h.getElementsByClassName||h.getElementsByClassName("e").length===0)){h.lastChild.className="e";if(h.getElementsByClassName("e").length!==1){r.order.splice(1,0,"CLASS");r.find.CLASS=function(j,o,p){if(typeof o.getElementsByClassName!=="undefined"&&!p)return o.getElementsByClassName(j[1])};h=null}}})();n.contains=x.documentElement.contains?function(h,j){return h!==j&&(h.contains?h.contains(j):true)}:x.documentElement.compareDocumentPosition?
function(h,j){return!!(h.compareDocumentPosition(j)&16)}:function(){return false};n.isXML=function(h){return(h=(h?h.ownerDocument||h:0).documentElement)?h.nodeName!=="HTML":false};var O=function(h,j){var o,p=[],q="";for(j=j.nodeType?[j]:j;o=r.match.PSEUDO.exec(h);){q+=o[0];h=h.replace(r.match.PSEUDO,"")}h=r.relative[h]?h+"*":h;o=0;for(var t=j.length;o<t;o++)n(h,j[o],p);return n.filter(q,p)};c.find=n;c.expr=n.selectors;c.expr[":"]=c.expr.filters;c.unique=n.uniqueSort;c.text=n.getText;c.isXMLDoc=n.isXML;
c.contains=n.contains})();var Fb=/Until$/,Gb=/^(?:parents|prevUntil|prevAll)/,Hb=/,/,lb=/^.[^:#\[\.,]*$/,Ib=Array.prototype.slice,Ta=c.expr.match.POS,Jb={children:true,contents:true,next:true,prev:true};c.fn.extend({find:function(a){var b=this,d,e;if(typeof a!=="string")return c(a).filter(function(){d=0;for(e=b.length;d<e;d++)if(c.contains(b[d],this))return true});var f=this.pushStack("","find",a),g,i,l;d=0;for(e=this.length;d<e;d++){g=f.length;c.find(a,this[d],f);if(d>0)for(i=g;i<f.length;i++)for(l=
0;l<g;l++)if(f[l]===f[i]){f.splice(i--,1);break}}return f},has:function(a){var b=c(a);return this.filter(function(){for(var d=0,e=b.length;d<e;d++)if(c.contains(this,b[d]))return true})},not:function(a){return this.pushStack(Aa(this,a,false),"not",a)},filter:function(a){return this.pushStack(Aa(this,a,true),"filter",a)},is:function(a){return!!a&&(typeof a==="string"?c.filter(a,this).length>0:this.filter(a).length>0)},closest:function(a,b){var d=[],e,f,g=this[0];if(c.isArray(a)){var i,l={},m=1;if(g&&
a.length){e=0;for(f=a.length;e<f;e++){i=a[e];l[i]||(l[i]=Ta.test(i)?c(i,b||this.context):i)}for(;g&&g.ownerDocument&&g!==b;){for(i in l){a=l[i];if(a.jquery?a.index(g)>-1:c(g).is(a))d.push({selector:i,elem:g,level:m})}g=g.parentNode;m++}}return d}i=Ta.test(a)||typeof a!=="string"?c(a,b||this.context):0;e=0;for(f=this.length;e<f;e++)for(g=this[e];g;)if(i?i.index(g)>-1:c.find.matchesSelector(g,a)){d.push(g);break}else{g=g.parentNode;if(!g||!g.ownerDocument||g===b||g.nodeType===11)break}d=d.length>1?
c.unique(d):d;return this.pushStack(d,"closest",a)},index:function(a){if(!a||typeof a==="string")return c.inArray(this[0],a?c(a):this.parent().children());return c.inArray(a.jquery?a[0]:a,this)},add:function(a,b){a=typeof a==="string"?c(a,b):c.makeArray(a&&a.nodeType?[a]:a);b=c.merge(this.get(),a);return this.pushStack(za(a[0])||za(b[0])?b:c.unique(b))},andSelf:function(){return this.add(this.prevObject)}});c.each({parent:function(a){return(a=a.parentNode)&&a.nodeType!==11?a:null},parents:function(a){return c.dir(a,
"parentNode")},parentsUntil:function(a,b,d){return c.dir(a,"parentNode",d)},next:function(a){return c.nth(a,2,"nextSibling")},prev:function(a){return c.nth(a,2,"previousSibling")},nextAll:function(a){return c.dir(a,"nextSibling")},prevAll:function(a){return c.dir(a,"previousSibling")},nextUntil:function(a,b,d){return c.dir(a,"nextSibling",d)},prevUntil:function(a,b,d){return c.dir(a,"previousSibling",d)},siblings:function(a){return c.sibling(a.parentNode.firstChild,a)},children:function(a){return c.sibling(a.firstChild)},
contents:function(a){return c.nodeName(a,"iframe")?a.contentDocument||a.contentWindow.document:c.makeArray(a.childNodes)}},function(a,b){c.fn[a]=function(d,e){var f=c.map(this,b,d),g=Ib.call(arguments);Fb.test(a)||(e=d);if(e&&typeof e==="string")f=c.filter(e,f);f=this.length>1&&!Jb[a]?c.unique(f):f;if((this.length>1||Hb.test(e))&&Gb.test(a))f=f.reverse();return this.pushStack(f,a,g.join(","))}});c.extend({filter:function(a,b,d){if(d)a=":not("+a+")";return b.length===1?c.find.matchesSelector(b[0],
a)?[b[0]]:[]:c.find.matches(a,b)},dir:function(a,b,d){var e=[];for(a=a[b];a&&a.nodeType!==9&&(d===v||a.nodeType!==1||!c(a).is(d));){a.nodeType===1&&e.push(a);a=a[b]}return e},nth:function(a,b,d){b=b||1;for(var e=0;a;a=a[d])if(a.nodeType===1&&++e===b)break;return a},sibling:function(a,b){for(var d=[];a;a=a.nextSibling)a.nodeType===1&&a!==b&&d.push(a);return d}});var Kb=/ jQuery\d+="(?:\d+|null)"/g,ta=/^\s+/,Ua=/<(?!area|br|col|embed|hr|img|input|link|meta|param)(([\w:]+)[^>]*)\/>/ig,Va=/<([\w:]+)/,
Lb=/<tbody/i,Mb=/<|&#?\w+;/,Wa=/<(?:script|object|embed|option|style)/i,Xa=/checked\s*(?:[^=]|=\s*.checked.)/i,Nb=/\/(java|ecma)script/i,ob=/^\s*<!(?:\[CDATA\[|\-\-)/,N={option:[1,"<select multiple='multiple'>","</select>"],legend:[1,"<fieldset>","</fieldset>"],thead:[1,"<table>","</table>"],tr:[2,"<table><tbody>","</tbody></table>"],td:[3,"<table><tbody><tr>","</tr></tbody></table>"],col:[2,"<table><tbody></tbody><colgroup>","</colgroup></table>"],area:[1,"<map>","</map>"],_default:[0,"",""]};N.optgroup=
N.option;N.tbody=N.tfoot=N.colgroup=N.caption=N.thead;N.th=N.td;if(!c.support.htmlSerialize)N._default=[1,"div<div>","</div>"];c.fn.extend({text:function(a){if(c.isFunction(a))return this.each(function(b){var d=c(this);d.text(a.call(this,b,d.text()))});if(typeof a!=="object"&&a!==v)return this.empty().append((this[0]&&this[0].ownerDocument||x).createTextNode(a));return c.text(this)},wrapAll:function(a){if(c.isFunction(a))return this.each(function(d){c(this).wrapAll(a.call(this,d))});if(this[0]){var b=
c(a,this[0].ownerDocument).eq(0).clone(true);this[0].parentNode&&b.insertBefore(this[0]);b.map(function(){for(var d=this;d.firstChild&&d.firstChild.nodeType===1;)d=d.firstChild;return d}).append(this)}return this},wrapInner:function(a){if(c.isFunction(a))return this.each(function(b){c(this).wrapInner(a.call(this,b))});return this.each(function(){var b=c(this),d=b.contents();d.length?d.wrapAll(a):b.append(a)})},wrap:function(a){return this.each(function(){c(this).wrapAll(a)})},unwrap:function(){return this.parent().each(function(){c.nodeName(this,
"body")||c(this).replaceWith(this.childNodes)}).end()},append:function(){return this.domManip(arguments,true,function(a){this.nodeType===1&&this.appendChild(a)})},prepend:function(){return this.domManip(arguments,true,function(a){this.nodeType===1&&this.insertBefore(a,this.firstChild)})},before:function(){if(this[0]&&this[0].parentNode)return this.domManip(arguments,false,function(b){this.parentNode.insertBefore(b,this)});else if(arguments.length){var a=c(arguments[0]);a.push.apply(a,this.toArray());
return this.pushStack(a,"before",arguments)}},after:function(){if(this[0]&&this[0].parentNode)return this.domManip(arguments,false,function(b){this.parentNode.insertBefore(b,this.nextSibling)});else if(arguments.length){var a=this.pushStack(this,"after",arguments);a.push.apply(a,c(arguments[0]).toArray());return a}},remove:function(a,b){for(var d=0,e;(e=this[d])!=null;d++)if(!a||c.filter(a,[e]).length){if(!b&&e.nodeType===1){c.cleanData(e.getElementsByTagName("*"));c.cleanData([e])}e.parentNode&&
e.parentNode.removeChild(e)}return this},empty:function(){for(var a=0,b;(b=this[a])!=null;a++)for(b.nodeType===1&&c.cleanData(b.getElementsByTagName("*"));b.firstChild;)b.removeChild(b.firstChild);return this},clone:function(a,b){a=a==null?false:a;b=b==null?a:b;return this.map(function(){return c.clone(this,a,b)})},html:function(a){if(a===v)return this[0]&&this[0].nodeType===1?this[0].innerHTML.replace(Kb,""):null;else if(typeof a==="string"&&!Wa.test(a)&&(c.support.leadingWhitespace||!ta.test(a))&&
!N[(Va.exec(a)||["",""])[1].toLowerCase()]){a=a.replace(Ua,"<$1></$2>");try{for(var b=0,d=this.length;b<d;b++)if(this[b].nodeType===1){c.cleanData(this[b].getElementsByTagName("*"));this[b].innerHTML=a}}catch(e){this.empty().append(a)}}else c.isFunction(a)?this.each(function(f){var g=c(this);g.html(a.call(this,f,g.html()))}):this.empty().append(a);return this},replaceWith:function(a){if(this[0]&&this[0].parentNode){if(c.isFunction(a))return this.each(function(b){var d=c(this),e=d.html();d.replaceWith(a.call(this,
b,e))});if(typeof a!=="string")a=c(a).detach();return this.each(function(){var b=this.nextSibling,d=this.parentNode;c(this).remove();b?c(b).before(a):c(d).append(a)})}else return this.length?this.pushStack(c(c.isFunction(a)?a():a),"replaceWith",a):this},detach:function(a){return this.remove(a,true)},domManip:function(a,b,d){var e,f,g,i=a[0],l=[];if(!c.support.checkClone&&arguments.length===3&&typeof i==="string"&&Xa.test(i))return this.each(function(){c(this).domManip(a,b,d,true)});if(c.isFunction(i))return this.each(function(A){var B=
c(this);a[0]=i.call(this,A,b?B.html():v);B.domManip(a,b,d)});if(this[0]){e=i&&i.parentNode;e=c.support.parentNode&&e&&e.nodeType===11&&e.childNodes.length===this.length?{fragment:e}:c.buildFragment(a,this,l);g=e.fragment;if(f=g.childNodes.length===1?(g=g.firstChild):g.firstChild){b=b&&c.nodeName(f,"tr");for(var m=0,n=this.length,r=n-1;m<n;m++)d.call(b?mb(this[m],f):this[m],e.cacheable||n>1&&m<r?c.clone(g,true,true):g)}l.length&&c.each(l,nb)}return this}});c.buildFragment=function(a,b,d){var e,f,g;
b=b&&b[0]?b[0].ownerDocument||b[0]:x;if(a.length===1&&typeof a[0]==="string"&&a[0].length<512&&b===x&&a[0].charAt(0)==="<"&&!Wa.test(a[0])&&(c.support.checkClone||!Xa.test(a[0]))){f=true;if((g=c.fragments[a[0]])&&g!==1)e=g}if(!e){e=b.createDocumentFragment();c.clean(a,b,e,d)}if(f)c.fragments[a[0]]=g?e:1;return{fragment:e,cacheable:f}};c.fragments={};c.each({appendTo:"append",prependTo:"prepend",insertBefore:"before",insertAfter:"after",replaceAll:"replaceWith"},function(a,b){c.fn[a]=function(d){var e=
[];d=c(d);var f=this.length===1&&this[0].parentNode;if(f&&f.nodeType===11&&f.childNodes.length===1&&d.length===1){d[b](this[0]);return this}else{f=0;for(var g=d.length;f<g;f++){var i=(f>0?this.clone(true):this).get();c(d[f])[b](i);e=e.concat(i)}return this.pushStack(e,a,d.selector)}}});c.extend({clone:function(a,b,d){var e=a.cloneNode(true),f,g,i;if((!c.support.noCloneEvent||!c.support.noCloneChecked)&&(a.nodeType===1||a.nodeType===11)&&!c.isXMLDoc(a)){Ca(a,e);f=ca(a);g=ca(e);for(i=0;f[i];++i)Ca(f[i],
g[i])}if(b){Ba(a,e);if(d){f=ca(a);g=ca(e);for(i=0;f[i];++i)Ba(f[i],g[i])}}return e},clean:function(a,b,d,e){b=b||x;if(typeof b.createElement==="undefined")b=b.ownerDocument||b[0]&&b[0].ownerDocument||x;for(var f=[],g,i=0,l;(l=a[i])!=null;i++){if(typeof l==="number")l+="";if(l){if(typeof l==="string")if(Mb.test(l)){l=l.replace(Ua,"<$1></$2>");g=(Va.exec(l)||["",""])[1].toLowerCase();var m=N[g]||N._default,n=m[0],r=b.createElement("div");for(r.innerHTML=m[1]+l+m[2];n--;)r=r.lastChild;if(!c.support.tbody){n=
Lb.test(l);m=g==="table"&&!n?r.firstChild&&r.firstChild.childNodes:m[1]==="<table>"&&!n?r.childNodes:[];for(g=m.length-1;g>=0;--g)c.nodeName(m[g],"tbody")&&!m[g].childNodes.length&&m[g].parentNode.removeChild(m[g])}!c.support.leadingWhitespace&&ta.test(l)&&r.insertBefore(b.createTextNode(ta.exec(l)[0]),r.firstChild);l=r.childNodes}else l=b.createTextNode(l);var A;if(!c.support.appendChecked)if(l[0]&&typeof(A=l.length)==="number")for(g=0;g<A;g++)Ea(l[g]);else Ea(l);if(l.nodeType)f.push(l);else f=c.merge(f,
l)}}if(d){a=function(B){return!B.type||Nb.test(B.type)};for(i=0;f[i];i++)if(e&&c.nodeName(f[i],"script")&&(!f[i].type||f[i].type.toLowerCase()==="text/javascript"))e.push(f[i].parentNode?f[i].parentNode.removeChild(f[i]):f[i]);else{if(f[i].nodeType===1){b=c.grep(f[i].getElementsByTagName("script"),a);f.splice.apply(f,[i+1,0].concat(b))}d.appendChild(f[i])}}return f},cleanData:function(a){for(var b,d,e=c.cache,f=c.expando,g=c.event.special,i=c.support.deleteExpando,l=0,m;(m=a[l])!=null;l++)if(!(m.nodeName&&
c.noData[m.nodeName.toLowerCase()]))if(d=m[c.expando]){if((b=e[d]&&e[d][f])&&b.events){for(var n in b.events)g[n]?c.event.remove(m,n):c.removeEvent(m,n,b.handle);if(b.handle)b.handle.elem=null}if(i)delete m[c.expando];else m.removeAttribute&&m.removeAttribute(c.expando);delete e[d]}}});var Ya=/alpha\([^)]*\)/i,Ob=/opacity=([^)]*)/,Pb=/-([a-z])/ig,Qb=/([A-Z]|^ms)/g,Za=/^-?\d+(?:px)?$/i,Rb=/^-?\d/,Sb=/^[+\-]=/,Tb=/[^+\-\.\de]+/g,Ub={position:"absolute",visibility:"hidden",display:"block"},pb=["Left",
"Right"],qb=["Top","Bottom"],W,$a,ga,Vb=function(a,b){return b.toUpperCase()};c.fn.css=function(a,b){if(arguments.length===2&&b===v)return this;return c.access(this,a,b,true,function(d,e,f){return f!==v?c.style(d,e,f):c.css(d,e)})};c.extend({cssHooks:{opacity:{get:function(a,b){if(b){a=W(a,"opacity","opacity");return a===""?"1":a}else return a.style.opacity}}},cssNumber:{zIndex:true,fontWeight:true,opacity:true,zoom:true,lineHeight:true,widows:true,orphans:true},cssProps:{"float":c.support.cssFloat?
"cssFloat":"styleFloat"},style:function(a,b,d,e){if(!(!a||a.nodeType===3||a.nodeType===8||!a.style)){var f,g=c.camelCase(b),i=a.style,l=c.cssHooks[g];b=c.cssProps[g]||g;if(d!==v){e=typeof d;if(!(e==="number"&&isNaN(d)||d==null)){if(e==="string"&&Sb.test(d))d=+d.replace(Tb,"")+parseFloat(c.css(a,b));if(e==="number"&&!c.cssNumber[g])d+="px";if(!l||!("set"in l)||(d=l.set(a,d))!==v)try{i[b]=d}catch(m){}}}else{if(l&&"get"in l&&(f=l.get(a,false,e))!==v)return f;return i[b]}}},css:function(a,b,d){var e,
f;b=c.camelCase(b);f=c.cssHooks[b];b=c.cssProps[b]||b;if(b==="cssFloat")b="float";if(f&&"get"in f&&(e=f.get(a,true,d))!==v)return e;else if(W)return W(a,b)},swap:function(a,b,d){var e={};for(var f in b){e[f]=a.style[f];a.style[f]=b[f]}d.call(a);for(f in b)a.style[f]=e[f]},camelCase:function(a){return a.replace(Pb,Vb)}});c.curCSS=c.css;c.each(["height","width"],function(a,b){c.cssHooks[b]={get:function(d,e,f){var g;if(e){if(d.offsetWidth!==0)g=Fa(d,b,f);else c.swap(d,Ub,function(){g=Fa(d,b,f)});if(g<=
0){g=W(d,b,b);if(g==="0px"&&ga)g=ga(d,b,b);if(g!=null)return g===""||g==="auto"?"0px":g}if(g<0||g==null){g=d.style[b];return g===""||g==="auto"?"0px":g}return typeof g==="string"?g:g+"px"}},set:function(d,e){if(Za.test(e)){e=parseFloat(e);if(e>=0)return e+"px"}else return e}}});if(!c.support.opacity)c.cssHooks.opacity={get:function(a,b){return Ob.test((b&&a.currentStyle?a.currentStyle.filter:a.style.filter)||"")?parseFloat(RegExp.$1)/100+"":b?"1":""},set:function(a,b){var d=a.style;a=a.currentStyle;
d.zoom=1;b=c.isNaN(b)?"":"alpha(opacity="+b*100+")";a=a&&a.filter||d.filter||"";d.filter=Ya.test(a)?a.replace(Ya,b):a+" "+b}};c(function(){if(!c.support.reliableMarginRight)c.cssHooks.marginRight={get:function(a,b){var d;c.swap(a,{display:"inline-block"},function(){d=b?W(a,"margin-right","marginRight"):a.style.marginRight});return d}}});if(x.defaultView&&x.defaultView.getComputedStyle)$a=function(a,b){var d,e;b=b.replace(Qb,"-$1").toLowerCase();if(!(e=a.ownerDocument.defaultView))return v;if(e=e.getComputedStyle(a,
null)){d=e.getPropertyValue(b);if(d===""&&!c.contains(a.ownerDocument.documentElement,a))d=c.style(a,b)}return d};if(x.documentElement.currentStyle)ga=function(a,b){var d,e=a.currentStyle&&a.currentStyle[b],f=a.runtimeStyle&&a.runtimeStyle[b],g=a.style;if(!Za.test(e)&&Rb.test(e)){d=g.left;if(f)a.runtimeStyle.left=a.currentStyle.left;g.left=b==="fontSize"?"1em":e||0;e=g.pixelLeft+"px";g.left=d;if(f)a.runtimeStyle.left=f}return e===""?"auto":e};W=$a||ga;if(c.expr&&c.expr.filters){c.expr.filters.hidden=
function(a){var b=a.offsetHeight;return a.offsetWidth===0&&b===0||!c.support.reliableHiddenOffsets&&(a.style.display||c.css(a,"display"))==="none"};c.expr.filters.visible=function(a){return!c.expr.filters.hidden(a)}}var Wb=/%20/g,rb=/\[\]$/,ab=/\r?\n/g,Xb=/#.*$/,Yb=/^(.*?):[ \t]*([^\r\n]*)\r?$/mg,Zb=/^(?:color|date|datetime|email|hidden|month|number|password|range|search|tel|text|time|url|week)$/i,$b=/^(?:GET|HEAD)$/,ac=/^\/\//,bb=/\?/,bc=/<script\b[^<]*(?:(?!<\/script>)<[^<]*)*<\/script>/gi,cc=/^(?:select|textarea)/i,
Ha=/\s+/,dc=/([?&])_=[^&]*/,cb=/^([\w\+\.\-]+:)(?:\/\/([^\/?#:]*)(?::(\d+))?)?/,db=c.fn.load,ka={},eb={},T,U;try{T=xb.href}catch(jc){T=x.createElement("a");T.href="";T=T.href}U=cb.exec(T.toLowerCase())||[];c.fn.extend({load:function(a,b,d){if(typeof a!=="string"&&db)return db.apply(this,arguments);else if(!this.length)return this;var e=a.indexOf(" ");if(e>=0){var f=a.slice(e,a.length);a=a.slice(0,e)}e="GET";if(b)if(c.isFunction(b)){d=b;b=v}else if(typeof b==="object"){b=c.param(b,c.ajaxSettings.traditional);
e="POST"}var g=this;c.ajax({url:a,type:e,dataType:"html",data:b,complete:function(i,l,m){m=i.responseText;if(i.isResolved()){i.done(function(n){m=n});g.html(f?c("<div>").append(m.replace(bc,"")).find(f):m)}d&&g.each(d,[m,l,i])}});return this},serialize:function(){return c.param(this.serializeArray())},serializeArray:function(){return this.map(function(){return this.elements?c.makeArray(this.elements):this}).filter(function(){return this.name&&!this.disabled&&(this.checked||cc.test(this.nodeName)||
Zb.test(this.type))}).map(function(a,b){a=c(this).val();return a==null?null:c.isArray(a)?c.map(a,function(d){return{name:b.name,value:d.replace(ab,"\r\n")}}):{name:b.name,value:a.replace(ab,"\r\n")}}).get()}});c.each("ajaxStart ajaxStop ajaxComplete ajaxError ajaxSuccess ajaxSend".split(" "),function(a,b){c.fn[b]=function(d){return this.bind(b,d)}});c.each(["get","post"],function(a,b){c[b]=function(d,e,f,g){if(c.isFunction(e)){g=g||f;f=e;e=v}return c.ajax({type:b,url:d,data:e,success:f,dataType:g})}});
c.extend({getScript:function(a,b){return c.get(a,v,b,"script")},getJSON:function(a,b,d){return c.get(a,b,d,"json")},ajaxSetup:function(a,b){if(b)c.extend(true,a,c.ajaxSettings,b);else{b=a;a=c.extend(true,c.ajaxSettings,b)}for(var d in{context:1,url:1})if(d in b)a[d]=b[d];else if(d in c.ajaxSettings)a[d]=c.ajaxSettings[d];return a},ajaxSettings:{url:T,isLocal:/^(?:about|app|app\-storage|.+\-extension|file|widget):$/.test(U[1]),global:true,type:"GET",contentType:"application/x-www-form-urlencoded",
processData:true,async:true,accepts:{xml:"application/xml, text/xml",html:"text/html",text:"text/plain",json:"application/json, text/javascript","*":"*/*"},contents:{xml:/xml/,html:/html/,json:/json/},responseFields:{xml:"responseXML",text:"responseText"},converters:{"* text":E.String,"text html":true,"text json":c.parseJSON,"text xml":c.parseXML}},ajaxPrefilter:Ga(ka),ajaxTransport:Ga(eb),ajax:function(a,b){function d(p,q,t,y){if(I!==2){I=2;R&&clearTimeout(R);G=v;B=y||"";h.readyState=p?4:0;var w,
D,L;t=t?sb(e,h,t):v;if(p>=200&&p<300||p===304){if(e.ifModified){if(y=h.getResponseHeader("Last-Modified"))c.lastModified[n]=y;if(y=h.getResponseHeader("Etag"))c.etag[n]=y}if(p===304){q="notmodified";w=true}else try{D=tb(e,t);q="success";w=true}catch(k){q="parsererror";L=k}}else{L=q;if(!q||p){q="error";if(p<0)p=0}}h.status=p;h.statusText=q;w?i.resolveWith(f,[D,q,h]):i.rejectWith(f,[h,q,L]);h.statusCode(m);m=v;if(M)g.trigger("ajax"+(w?"Success":"Error"),[h,e,w?D:L]);l.resolveWith(f,[h,q]);if(M){g.trigger("ajaxComplete",
[h,e]);--c.active||c.event.trigger("ajaxStop")}}}if(typeof a==="object"){b=a;a=v}b=b||{};var e=c.ajaxSetup({},b),f=e.context||e,g=f!==e&&(f.nodeType||f instanceof c)?c(f):c.event,i=c.Deferred(),l=c._Deferred(),m=e.statusCode||{},n,r={},A={},B,C,G,R,I=0,M,O,h={readyState:0,setRequestHeader:function(p,q){if(!I){var t=p.toLowerCase();p=A[t]=A[t]||p;r[p]=q}return this},getAllResponseHeaders:function(){return I===2?B:null},getResponseHeader:function(p){var q;if(I===2){if(!C)for(C={};q=Yb.exec(B);)C[q[1].toLowerCase()]=
q[2];q=C[p.toLowerCase()]}return q===v?null:q},overrideMimeType:function(p){if(!I)e.mimeType=p;return this},abort:function(p){p=p||"abort";G&&G.abort(p);d(0,p);return this}};i.promise(h);h.success=h.done;h.error=h.fail;h.complete=l.done;h.statusCode=function(p){if(p){var q;if(I<2)for(q in p)m[q]=[m[q],p[q]];else{q=p[h.status];h.then(q,q)}}return this};e.url=((a||e.url)+"").replace(Xb,"").replace(ac,U[1]+"//");e.dataTypes=c.trim(e.dataType||"*").toLowerCase().split(Ha);if(e.crossDomain==null){a=cb.exec(e.url.toLowerCase());
e.crossDomain=!!(a&&(a[1]!=U[1]||a[2]!=U[2]||(a[3]||(a[1]==="http:"?80:443))!=(U[3]||(U[1]==="http:"?80:443))))}if(e.data&&e.processData&&typeof e.data!=="string")e.data=c.param(e.data,e.traditional);da(ka,e,b,h);if(I===2)return false;M=e.global;e.type=e.type.toUpperCase();e.hasContent=!$b.test(e.type);M&&c.active++===0&&c.event.trigger("ajaxStart");if(!e.hasContent){if(e.data)e.url+=(bb.test(e.url)?"&":"?")+e.data;n=e.url;if(e.cache===false){a=c.now();var j=e.url.replace(dc,"$1_="+a);e.url=j+(j===
e.url?(bb.test(e.url)?"&":"?")+"_="+a:"")}}if(e.data&&e.hasContent&&e.contentType!==false||b.contentType)h.setRequestHeader("Content-Type",e.contentType);if(e.ifModified){n=n||e.url;c.lastModified[n]&&h.setRequestHeader("If-Modified-Since",c.lastModified[n]);c.etag[n]&&h.setRequestHeader("If-None-Match",c.etag[n])}h.setRequestHeader("Accept",e.dataTypes[0]&&e.accepts[e.dataTypes[0]]?e.accepts[e.dataTypes[0]]+(e.dataTypes[0]!=="*"?", */*; q=0.01":""):e.accepts["*"]);for(O in e.headers)h.setRequestHeader(O,
e.headers[O]);if(e.beforeSend&&(e.beforeSend.call(f,h,e)===false||I===2)){h.abort();return false}for(O in{success:1,error:1,complete:1})h[O](e[O]);if(G=da(eb,e,b,h)){h.readyState=1;M&&g.trigger("ajaxSend",[h,e]);if(e.async&&e.timeout>0)R=setTimeout(function(){h.abort("timeout")},e.timeout);try{I=1;G.send(r,d)}catch(o){status<2?d(-1,o):c.error(o)}}else d(-1,"No Transport");return h},param:function(a,b){var d=[],e=function(g,i){i=c.isFunction(i)?i():i;d[d.length]=encodeURIComponent(g)+"="+encodeURIComponent(i)};
if(b===v)b=c.ajaxSettings.traditional;if(c.isArray(a)||a.jquery&&!c.isPlainObject(a))c.each(a,function(){e(this.name,this.value)});else for(var f in a)la(f,a[f],b,e);return d.join("&").replace(Wb,"+")}});c.extend({active:0,lastModified:{},etag:{}});var ec=c.now(),ha=/(\=)\?(&|$)|\?\?/i;c.ajaxSetup({jsonp:"callback",jsonpCallback:function(){return c.expando+"_"+ec++}});c.ajaxPrefilter("json jsonp",function(a,b,d){b=a.contentType==="application/x-www-form-urlencoded"&&typeof a.data==="string";if(a.dataTypes[0]===
"jsonp"||a.jsonp!==false&&(ha.test(a.url)||b&&ha.test(a.data))){var e,f=a.jsonpCallback=c.isFunction(a.jsonpCallback)?a.jsonpCallback():a.jsonpCallback,g=E[f],i=a.url,l=a.data,m="$1"+f+"$2";if(a.jsonp!==false){i=i.replace(ha,m);if(a.url===i){if(b)l=l.replace(ha,m);if(a.data===l)i+=(/\?/.test(i)?"&":"?")+a.jsonp+"="+f}}a.url=i;a.data=l;E[f]=function(n){e=[n]};d.always(function(){E[f]=g;e&&c.isFunction(g)&&E[f](e[0])});a.converters["script json"]=function(){e||c.error(f+" was not called");return e[0]};
a.dataTypes[0]="json";return"script"}});c.ajaxSetup({accepts:{script:"text/javascript, application/javascript, application/ecmascript, application/x-ecmascript"},contents:{script:/javascript|ecmascript/},converters:{"text script":function(a){c.globalEval(a);return a}}});c.ajaxPrefilter("script",function(a){if(a.cache===v)a.cache=false;if(a.crossDomain){a.type="GET";a.global=false}});c.ajaxTransport("script",function(a){if(a.crossDomain){var b,d=x.head||x.getElementsByTagName("head")[0]||x.documentElement;
return{send:function(e,f){b=x.createElement("script");b.async="async";if(a.scriptCharset)b.charset=a.scriptCharset;b.src=a.url;b.onload=b.onreadystatechange=function(g,i){if(i||!b.readyState||/loaded|complete/.test(b.readyState)){b.onload=b.onreadystatechange=null;d&&b.parentNode&&d.removeChild(b);b=v;i||f(200,"success")}};d.insertBefore(b,d.firstChild)},abort:function(){b&&b.onload(0,1)}}}});var ua=E.ActiveXObject?function(){for(var a in X)X[a](0,1)}:false,fc=0,X;c.ajaxSettings.xhr=E.ActiveXObject?
function(){return!this.isLocal&&Ia()||ub()}:Ia;(function(a){c.extend(c.support,{ajax:!!a,cors:!!a&&"withCredentials"in a})})(c.ajaxSettings.xhr());c.support.ajax&&c.ajaxTransport(function(a){if(!a.crossDomain||c.support.cors){var b;return{send:function(d,e){var f=a.xhr(),g,i;a.username?f.open(a.type,a.url,a.async,a.username,a.password):f.open(a.type,a.url,a.async);if(a.xhrFields)for(i in a.xhrFields)f[i]=a.xhrFields[i];a.mimeType&&f.overrideMimeType&&f.overrideMimeType(a.mimeType);if(!a.crossDomain&&
!d["X-Requested-With"])d["X-Requested-With"]="XMLHttpRequest";try{for(i in d)f.setRequestHeader(i,d[i])}catch(l){}f.send(a.hasContent&&a.data||null);b=function(m,n){var r,A,B,C,G;try{if(b&&(n||f.readyState===4)){b=v;if(g){f.onreadystatechange=c.noop;ua&&delete X[g]}if(n)f.readyState!==4&&f.abort();else{r=f.status;B=f.getAllResponseHeaders();C={};if((G=f.responseXML)&&G.documentElement)C.xml=G;C.text=f.responseText;try{A=f.statusText}catch(R){A=""}if(!r&&a.isLocal&&!a.crossDomain)r=C.text?200:404;
else if(r===1223)r=204}}}catch(I){n||e(-1,I)}C&&e(r,A,C,B)};if(!a.async||f.readyState===4)b();else{g=++fc;if(ua){if(!X){X={};c(E).unload(ua)}X[g]=b}f.onreadystatechange=b}},abort:function(){b&&b(0,1)}}}});var ma={},P,Z,gc=/^(?:toggle|show|hide)$/,hc=/^([+\-]=)?([\d+.\-]+)([a-z%]*)$/i,Y,Ka=[["height","marginTop","marginBottom","paddingTop","paddingBottom"],["width","marginLeft","marginRight","paddingLeft","paddingRight"],["opacity"]],ea,va=E.webkitRequestAnimationFrame||E.mozRequestAnimationFrame||
E.oRequestAnimationFrame;c.fn.extend({show:function(a,b,d){if(a||a===0)return this.animate(V("show",3),a,b,d);else{d=0;for(var e=this.length;d<e;d++){a=this[d];if(a.style){b=a.style.display;if(!c._data(a,"olddisplay")&&b==="none")b=a.style.display="";b===""&&c.css(a,"display")==="none"&&c._data(a,"olddisplay",La(a.nodeName))}}for(d=0;d<e;d++){a=this[d];if(a.style){b=a.style.display;if(b===""||b==="none")a.style.display=c._data(a,"olddisplay")||""}}return this}},hide:function(a,b,d){if(a||a===0)return this.animate(V("hide",
3),a,b,d);else{a=0;for(b=this.length;a<b;a++)if(this[a].style){d=c.css(this[a],"display");d!=="none"&&!c._data(this[a],"olddisplay")&&c._data(this[a],"olddisplay",d)}for(a=0;a<b;a++)if(this[a].style)this[a].style.display="none";return this}},_toggle:c.fn.toggle,toggle:function(a,b,d){var e=typeof a==="boolean";if(c.isFunction(a)&&c.isFunction(b))this._toggle.apply(this,arguments);else a==null||e?this.each(function(){var f=e?a:c(this).is(":hidden");c(this)[f?"show":"hide"]()}):this.animate(V("toggle",
3),a,b,d);return this},fadeTo:function(a,b,d,e){return this.filter(":hidden").css("opacity",0).show().end().animate({opacity:b},a,d,e)},animate:function(a,b,d,e){var f=c.speed(b,d,e);if(c.isEmptyObject(a))return this.each(f.complete,[false]);a=c.extend({},a);return this[f.queue===false?"each":"queue"](function(){f.queue===false&&c._mark(this);var g=c.extend({},f),i=this.nodeType===1,l=i&&c(this).is(":hidden"),m,n,r,A,B;g.animatedProperties={};for(r in a){m=c.camelCase(r);if(r!==m){a[m]=a[r];delete a[r]}n=
a[m];if(c.isArray(n)){g.animatedProperties[m]=n[1];n=a[m]=n[0]}else g.animatedProperties[m]=g.specialEasing&&g.specialEasing[m]||g.easing||"swing";if(n==="hide"&&l||n==="show"&&!l)return g.complete.call(this);if(i&&(m==="height"||m==="width")){g.overflow=[this.style.overflow,this.style.overflowX,this.style.overflowY];if(c.css(this,"display")==="inline"&&c.css(this,"float")==="none")if(c.support.inlineBlockNeedsLayout){n=La(this.nodeName);if(n==="inline")this.style.display="inline-block";else{this.style.display=
"inline";this.style.zoom=1}}else this.style.display="inline-block"}}if(g.overflow!=null)this.style.overflow="hidden";for(r in a){i=new c.fx(this,g,r);n=a[r];if(gc.test(n))i[n==="toggle"?l?"show":"hide":n]();else{m=hc.exec(n);A=i.cur();if(m){n=parseFloat(m[2]);B=m[3]||(c.cssNumber[r]?"":"px");if(B!=="px"){c.style(this,r,(n||1)+B);A=(n||1)/i.cur()*A;c.style(this,r,A+B)}if(m[1])n=(m[1]==="-="?-1:1)*n+A;i.custom(A,n,B)}else i.custom(A,n,"")}}return true})},stop:function(a,b){a&&this.queue([]);this.each(function(){var d=
c.timers,e=d.length;for(b||c._unmark(true,this);e--;)if(d[e].elem===this){b&&d[e](true);d.splice(e,1)}});b||this.dequeue();return this}});c.each({slideDown:V("show",1),slideUp:V("hide",1),slideToggle:V("toggle",1),fadeIn:{opacity:"show"},fadeOut:{opacity:"hide"},fadeToggle:{opacity:"toggle"}},function(a,b){c.fn[a]=function(d,e,f){return this.animate(b,d,e,f)}});c.extend({speed:function(a,b,d){var e=a&&typeof a==="object"?c.extend({},a):{complete:d||!d&&b||c.isFunction(a)&&a,duration:a,easing:d&&b||
b&&!c.isFunction(b)&&b};e.duration=c.fx.off?0:typeof e.duration==="number"?e.duration:e.duration in c.fx.speeds?c.fx.speeds[e.duration]:c.fx.speeds._default;e.old=e.complete;e.complete=function(f){if(e.queue!==false)c.dequeue(this);else f!==false&&c._unmark(this);c.isFunction(e.old)&&e.old.call(this)};return e},easing:{linear:function(a,b,d,e){return d+e*a},swing:function(a,b,d,e){return(-Math.cos(a*Math.PI)/2+0.5)*e+d}},timers:[],fx:function(a,b,d){this.options=b;this.elem=a;this.prop=d;b.orig=b.orig||
{}}});c.fx.prototype={update:function(){this.options.step&&this.options.step.call(this.elem,this.now,this);(c.fx.step[this.prop]||c.fx.step._default)(this)},cur:function(){if(this.elem[this.prop]!=null&&(!this.elem.style||this.elem.style[this.prop]==null))return this.elem[this.prop];var a,b=c.css(this.elem,this.prop);return isNaN(a=parseFloat(b))?!b||b==="auto"?0:b:a},custom:function(a,b,d){function e(l){return f.step(l)}var f=this,g=c.fx,i;this.startTime=ea||Ja();this.start=a;this.end=b;this.unit=
d||this.unit||(c.cssNumber[this.prop]?"":"px");this.now=this.start;this.pos=this.state=0;e.elem=this.elem;if(e()&&c.timers.push(e)&&!Y)if(va){Y=1;i=function(){if(Y){va(i);g.tick()}};va(i)}else Y=setInterval(g.tick,g.interval)},show:function(){this.options.orig[this.prop]=c.style(this.elem,this.prop);this.options.show=true;this.custom(this.prop==="width"||this.prop==="height"?1:0,this.cur());c(this.elem).show()},hide:function(){this.options.orig[this.prop]=c.style(this.elem,this.prop);this.options.hide=
true;this.custom(this.cur(),0)},step:function(a){var b=ea||Ja(),d=true,e=this.elem,f=this.options,g;if(a||b>=f.duration+this.startTime){this.now=this.end;this.pos=this.state=1;this.update();f.animatedProperties[this.prop]=true;for(g in f.animatedProperties)if(f.animatedProperties[g]!==true)d=false;if(d){f.overflow!=null&&!c.support.shrinkWrapBlocks&&c.each(["","X","Y"],function(l,m){e.style["overflow"+m]=f.overflow[l]});f.hide&&c(e).hide();if(f.hide||f.show)for(var i in f.animatedProperties)c.style(e,
i,f.orig[i]);f.complete.call(e)}return false}else{if(f.duration==Infinity)this.now=b;else{a=b-this.startTime;this.state=a/f.duration;this.pos=c.easing[f.animatedProperties[this.prop]](this.state,a,0,1,f.duration);this.now=this.start+(this.end-this.start)*this.pos}this.update()}return true}};c.extend(c.fx,{tick:function(){for(var a=c.timers,b=0;b<a.length;++b)a[b]()||a.splice(b--,1);a.length||c.fx.stop()},interval:13,stop:function(){clearInterval(Y);Y=null},speeds:{slow:600,fast:200,_default:400},
step:{opacity:function(a){c.style(a.elem,"opacity",a.now)},_default:function(a){if(a.elem.style&&a.elem.style[a.prop]!=null)a.elem.style[a.prop]=(a.prop==="width"||a.prop==="height"?Math.max(0,a.now):a.now)+a.unit;else a.elem[a.prop]=a.now}}});if(c.expr&&c.expr.filters)c.expr.filters.animated=function(a){return c.grep(c.timers,function(b){return a===b.elem}).length};var ic=/^t(?:able|d|h)$/i,fb=/^(?:body|html)$/i;c.fn.offset="getBoundingClientRect"in x.documentElement?function(a){var b=this[0],d;
if(a)return this.each(function(i){c.offset.setOffset(this,a,i)});if(!b||!b.ownerDocument)return null;if(b===b.ownerDocument.body)return c.offset.bodyOffset(b);try{d=b.getBoundingClientRect()}catch(e){}var f=b.ownerDocument,g=f.documentElement;if(!d||!c.contains(g,b))return d?{top:d.top,left:d.left}:{top:0,left:0};b=f.body;f=na(f);return{top:d.top+(f.pageYOffset||c.support.boxModel&&g.scrollTop||b.scrollTop)-(g.clientTop||b.clientTop||0),left:d.left+(f.pageXOffset||c.support.boxModel&&g.scrollLeft||
b.scrollLeft)-(g.clientLeft||b.clientLeft||0)}}:function(a){var b=this[0];if(a)return this.each(function(r){c.offset.setOffset(this,a,r)});if(!b||!b.ownerDocument)return null;if(b===b.ownerDocument.body)return c.offset.bodyOffset(b);c.offset.initialize();var d,e=b.offsetParent,f=b,g=b.ownerDocument,i=g.documentElement,l=g.body;d=(g=g.defaultView)?g.getComputedStyle(b,null):b.currentStyle;for(var m=b.offsetTop,n=b.offsetLeft;(b=b.parentNode)&&b!==l&&b!==i;){if(c.offset.supportsFixedPosition&&d.position===
"fixed")break;d=g?g.getComputedStyle(b,null):b.currentStyle;m-=b.scrollTop;n-=b.scrollLeft;if(b===e){m+=b.offsetTop;n+=b.offsetLeft;if(c.offset.doesNotAddBorder&&!(c.offset.doesAddBorderForTableAndCells&&ic.test(b.nodeName))){m+=parseFloat(d.borderTopWidth)||0;n+=parseFloat(d.borderLeftWidth)||0}f=e;e=b.offsetParent}if(c.offset.subtractsBorderForOverflowNotVisible&&d.overflow!=="visible"){m+=parseFloat(d.borderTopWidth)||0;n+=parseFloat(d.borderLeftWidth)||0}d=d}if(d.position==="relative"||d.position===
"static"){m+=l.offsetTop;n+=l.offsetLeft}if(c.offset.supportsFixedPosition&&d.position==="fixed"){m+=Math.max(i.scrollTop,l.scrollTop);n+=Math.max(i.scrollLeft,l.scrollLeft)}return{top:m,left:n}};c.offset={initialize:function(){var a=x.body,b=x.createElement("div"),d,e,f,g=parseFloat(c.css(a,"marginTop"))||0;c.extend(b.style,{position:"absolute",top:0,left:0,margin:0,border:0,width:"1px",height:"1px",visibility:"hidden"});b.innerHTML="<div style='position:absolute;top:0;left:0;margin:0;border:5px solid #000;padding:0;width:1px;height:1px;'><div></div></div><table style='position:absolute;top:0;left:0;margin:0;border:5px solid #000;padding:0;width:1px;height:1px;' cellpadding='0' cellspacing='0'><tr><td></td></tr></table>";
a.insertBefore(b,a.firstChild);d=b.firstChild;e=d.firstChild;f=d.nextSibling.firstChild.firstChild;this.doesNotAddBorder=e.offsetTop!==5;this.doesAddBorderForTableAndCells=f.offsetTop===5;e.style.position="fixed";e.style.top="20px";this.supportsFixedPosition=e.offsetTop===20||e.offsetTop===15;e.style.position=e.style.top="";d.style.overflow="hidden";d.style.position="relative";this.subtractsBorderForOverflowNotVisible=e.offsetTop===-5;this.doesNotIncludeMarginInBodyOffset=a.offsetTop!==g;a.removeChild(b);
c.offset.initialize=c.noop},bodyOffset:function(a){var b=a.offsetTop,d=a.offsetLeft;c.offset.initialize();if(c.offset.doesNotIncludeMarginInBodyOffset){b+=parseFloat(c.css(a,"marginTop"))||0;d+=parseFloat(c.css(a,"marginLeft"))||0}return{top:b,left:d}},setOffset:function(a,b,d){var e=c.css(a,"position");if(e==="static")a.style.position="relative";var f=c(a),g=f.offset(),i=c.css(a,"top"),l=c.css(a,"left"),m={},n={};if((e==="absolute"||e==="fixed")&&c.inArray("auto",[i,l])>-1){n=f.position();e=n.top;
l=n.left}else{e=parseFloat(i)||0;l=parseFloat(l)||0}if(c.isFunction(b))b=b.call(a,d,g);if(b.top!=null)m.top=b.top-g.top+e;if(b.left!=null)m.left=b.left-g.left+l;"using"in b?b.using.call(a,m):f.css(m)}};c.fn.extend({position:function(){if(!this[0])return null;var a=this[0],b=this.offsetParent(),d=this.offset(),e=fb.test(b[0].nodeName)?{top:0,left:0}:b.offset();d.top-=parseFloat(c.css(a,"marginTop"))||0;d.left-=parseFloat(c.css(a,"marginLeft"))||0;e.top+=parseFloat(c.css(b[0],"borderTopWidth"))||0;
e.left+=parseFloat(c.css(b[0],"borderLeftWidth"))||0;return{top:d.top-e.top,left:d.left-e.left}},offsetParent:function(){return this.map(function(){for(var a=this.offsetParent||x.body;a&&!fb.test(a.nodeName)&&c.css(a,"position")==="static";)a=a.offsetParent;return a})}});c.each(["Left","Top"],function(a,b){var d="scroll"+b;c.fn[d]=function(e){var f,g;if(e===v){f=this[0];if(!f)return null;return(g=na(f))?"pageXOffset"in g?g[a?"pageYOffset":"pageXOffset"]:c.support.boxModel&&g.document.documentElement[d]||
g.document.body[d]:f[d]}return this.each(function(){if(g=na(this))g.scrollTo(!a?e:c(g).scrollLeft(),a?e:c(g).scrollTop());else this[d]=e})}});c.each(["Height","Width"],function(a,b){var d=b.toLowerCase();c.fn["inner"+b]=function(){return this[0]?parseFloat(c.css(this[0],d,"padding")):null};c.fn["outer"+b]=function(e){return this[0]?parseFloat(c.css(this[0],d,e?"margin":"border")):null};c.fn[d]=function(e){var f=this[0];if(!f)return e==null?null:this;if(c.isFunction(e))return this.each(function(i){var l=
c(this);l[d](e.call(this,i,l[d]()))});if(c.isWindow(f)){var g=f.document.documentElement["client"+b];return f.document.compatMode==="CSS1Compat"&&g||f.document.body["client"+b]||g}else if(f.nodeType===9)return Math.max(f.documentElement["client"+b],f.body["scroll"+b],f.documentElement["scroll"+b],f.body["offset"+b],f.documentElement["offset"+b]);else if(e===v){f=c.css(f,d);g=parseFloat(f);return c.isNaN(g)?f:g}else return this.css(d,typeof e==="string"?e:e+"px")}});E.jQuery=E.$=c})(window);
;
steal.end();
steal.plugins("jquery").then(function(j){var g={undHash:/_|-/,colons:/::/,words:/([A-Z]+)([A-Z][a-z])/g,lowUp:/([a-z\d])([A-Z])/g,dash:/([a-z\d])([A-Z])/g,replacer:/\{([^\}]+)\}/g,dot:/\./},l=function(a,c,b){return a[c]||b&&(a[c]={})},m=function(a){return(a=typeof a)&&(a=="function"||a=="object")},n=function(a,c,b){a=a?a.split(g.dot):[];var f=a.length;c=j.isArray(c)?c:[c||window];var d,e,h,o=0;if(f==0)return c[0];for(;d=c[o++];){for(h=0;h<f-1&&m(d);h++)d=l(d,a[h],b);if(m(d)){e=l(d,a[h],b);if(e!==
undefined){b===false&&delete d[a[h]];return e}}}},k=j.String=j.extend(j.String||{},{getObject:n,capitalize:function(a){return a.charAt(0).toUpperCase()+a.substr(1)},camelize:function(a){a=k.classize(a);return a.charAt(0).toLowerCase()+a.substr(1)},classize:function(a,c){a=a.split(g.undHash);for(var b=0;b<a.length;b++)a[b]=k.capitalize(a[b]);return a.join(c||"")},niceName:function(){k.classize(parts[i]," ")},underscore:function(a){return a.replace(g.colons,"/").replace(g.words,"$1_$2").replace(g.lowUp,
"$1_$2").replace(g.dash,"_").toLowerCase()},sub:function(a,c,b){var f=[];f.push(a.replace(g.replacer,function(d,e){d=n(e,c,typeof b=="boolean"?!b:b);e=typeof d;if((e==="object"||e==="function")&&e!==null){f.push(d);return""}else return""+d}));return f.length<=1?f[0]:f}})});
;
steal.end();
steal.plugins("jquery/event").then(function(a){var e=jQuery.cleanData;a.cleanData=function(b){for(var c=0,d;(d=b[c])!==undefined;c++)a(d).triggerHandler("destroyed");e(b)}});
;
steal.end();
steal.plugins("jquery");
;
steal.end();
steal.plugins("jquery/controller","jquery/lang/openajax").then(function(){jQuery.Controller.processors.subscribe=function(d,e,a,b){var c=OpenAjax.hub.subscribe(a,b);return function(){OpenAjax.hub.unsubscribe(c)}};jQuery.Controller.prototype.publish=function(){OpenAjax.hub.publish.apply(OpenAjax.hub,arguments)}});
;
steal.end();
steal.then(function(){if(!window.OpenAjax){OpenAjax=new (function(){var d={};this.hub=d;d.implementer="http://openajax.org";d.implVersion="1.0";d.specVersion="1.0";d.implExtraData={};var h={};d.libraries=h;d.registerLibrary=function(a,c,b,e){h[a]={prefix:a,namespaceURI:c,version:b,extraData:e};this.publish("org.openajax.hub.registerLibrary",h[a])};d.unregisterLibrary=function(a){this.publish("org.openajax.hub.unregisterLibrary",h[a]);delete h[a]};d._subscriptions={c:{},s:[]};d._cleanup=[];d._subIndex=
0;d._pubDepth=0;d.subscribe=function(a,c,b,e,f){b||(b=window);var g=a+"."+this._subIndex;c={scope:b,cb:c,fcb:f,data:e,sid:this._subIndex++,hdl:g};this._subscribe(this._subscriptions,a.split("."),0,c);return g};d.publish=function(a,c){var b=a.split(".");this._pubDepth++;this._publish(this._subscriptions,b,0,a,c);this._pubDepth--;if(this._cleanup.length>0&&this._pubDepth==0){for(a=0;a<this._cleanup.length;a++)this.unsubscribe(this._cleanup[a].hdl);delete this._cleanup;this._cleanup=[]}};d.unsubscribe=
function(a){a=a.split(".");var c=a.pop();this._unsubscribe(this._subscriptions,a,0,c)};d._subscribe=function(a,c,b,e){var f=c[b];if(b==c.length)a.s.push(e);else{if(typeof a.c=="undefined")a.c={};if(typeof a.c[f]=="undefined")a.c[f]={c:{},s:[]};this._subscribe(a.c[f],c,b+1,e)}};d._publish=function(a,c,b,e,f,g,l){if(typeof a!="undefined"){if(b==c.length)a=a;else{this._publish(a.c[c[b]],c,b+1,e,f,g,l);this._publish(a.c["*"],c,b+1,e,f,g,l);a=a.c["**"]}if(typeof a!="undefined"){a=a.s;c=a.length;for(b=
0;b<c;b++)if(a[b].cb){var j=a[b].scope,k=a[b].cb,i=a[b].fcb,m=a[b].data,n=a[b].sid,o=a[b].cid;if(typeof k=="string")k=j[k];if(typeof i=="string")i=j[i];if(!i||i.call(j,e,f,m))if(!g||g(e,f,l,o))k.call(j,e,f,m,n)}}}};d._unsubscribe=function(a,c,b,e){if(typeof a!="undefined")if(b<c.length){var f=a.c[c[b]];this._unsubscribe(f,c,b+1,e);if(f.s.length==0){for(var g in f.c)return;delete a.c[c[b]]}}else{a=a.s;c=a.length;for(b=0;b<c;b++)if(e==a[b].sid){if(this._pubDepth>0){a[b].cb=null;this._cleanup.push(a[b])}else a.splice(b,
1);return}}};d.reinit=function(){for(var a in OpenAjax.hub.libraries)delete OpenAjax.hub.libraries[a];OpenAjax.hub.registerLibrary("OpenAjax","http://openajax.org/hub","1.0",{});delete OpenAjax._subscriptions;OpenAjax._subscriptions={c:{},s:[]};delete OpenAjax._cleanup;OpenAjax._cleanup=[];OpenAjax._subIndex=0;OpenAjax._pubDepth=0}});OpenAjax.hub.registerLibrary("OpenAjax","http://openajax.org/hub","1.0",{})}OpenAjax.hub.registerLibrary("JavaScriptMVC","http://JavaScriptMVC.com","3.0",{})});
;
steal.end();
steal.plugins("jquery/view","jquery/lang/rsplit").then(function(g){var p=function(a){eval(a)},q=function(a){return a.substr(0,a.length-1)},l=g.String.rsplit,j=g.extend,r=g.isArray,m=function(a){return a.replace(/\\/g,"\\\\").replace(/\n/g,"\\n").replace(/"/g,'\\"')};escapeHTML=function(a){return a.replace(/&/g,"&amp;").replace(/</g,"&lt;").replace(/>/g,"&gt;").replace(/"/g,"&#34;").replace(/'/g,"&#39;")};EJS=function(a){if(this.constructor!=EJS){var b=new EJS(a);return function(c,e){return b.render(c,
e)}}if(typeof a=="function"){this.template={};this.template.process=a}else{j(this,EJS.options,a);this.template=s(this.text,this.type,this.name)}};g.EJS=EJS;EJS.prototype={constructor:EJS,render:function(a,b){a=a||{};this._extra_helpers=b;b=new EJS.Helpers(a,b||{});return this.template.process.call(a,a,b)}};EJS.text=function(a){if(typeof a=="string")return a;if(a===null||a===undefined)return"";var b=a.hookup&&function(c,e){a.hookup.call(a,c,e)}||typeof a=="function"&&a||r(a)&&function(c,e){for(var d=
0;d<a.length;d++)a[d].hookup?a[d].hookup(c,e):a[d](c,e)};if(b)return"data-view-id='"+g.View.hookup(b)+"'";return a.toString?a.toString():""};EJS.clean=function(a){return typeof a=="string"?escapeHTML(a):""};var n=function(a,b,c){b=l(b,/\n/);for(var e=0;e<b.length;e++)t(a,b[e],c)},t=function(a,b,c){a.lines++;b=l(b,a.splitter);for(var e,d=0;d<b.length;d++){e=b[d];e!==null&&c(e,a)}},u=function(a,b){var c={};j(c,{left:a+"%",right:"%"+b,dLeft:a+"%%",dRight:"%%"+b,eeLeft:a+"%==",eLeft:a+"%=",cmnt:a+"%#",
cleanLeft:a+"%~",scan:n,lines:0});c.splitter=new RegExp("("+[c.dLeft,c.dRight,c.eeLeft,c.eLeft,c.cleanLeft,c.cmnt,c.left,c.right+"\n",c.right,"\n"].join(")|(").replace(/\[/g,"\\[").replace(/\]/g,"\\]")+")");return c},s=function(a,b,c){a=a.replace(/\r\n/g,"\n").replace(/\r/g,"\n");b=b||"<";var e=new EJS.Buffer(["var ___v1ew = [];"],[]),d="",o=function(h){e.push("___v1ew.push(",'"',m(h),'");')},i=null,k=function(){d=""};n(u(b,b==="["?"]":">"),a||"",function(h,f){if(i===null)switch(h){case "\n":d+="\n";
o(d);e.cr();k();break;case f.left:case f.eLeft:case f.eeLeft:case f.cleanLeft:case f.cmnt:i=h;d.length>0&&o(d);k();break;case f.dLeft:d+=f.left;break;default:d+=h;break}else switch(h){case f.right:switch(i){case f.left:if(d[d.length-1]=="\n"){d=q(d);e.push(d,";");e.cr()}else e.push(d,";");break;case f.cleanLeft:e.push("___v1ew.push(","(jQuery.EJS.clean(",d,")));");break;case f.eLeft:e.push("___v1ew.push(","(jQuery.EJS.text(",d,")));");break;case f.eeLeft:e.push("___v1ew.push(","(jQuery.EJS.text(",
d,")));");break}i=null;k();break;case f.dRight:d+=f.right;break;default:d+=h;break}});d.length>0&&e.push("___v1ew.push(",'"',m(d)+'");');a={out:"try { with(_VIEW) { with (_CONTEXT) {"+e.close()+" return ___v1ew.join('');}}}catch(e){e.lineNumber=null;throw e;}"};p.call(a,"this.process = (function(_CONTEXT,_VIEW){"+a.out+"});\r\n//@ sourceURL="+c+".js");return a};EJS.Buffer=function(a,b){this.line=[];this.script=[];this.post=b;this.push.apply(this,a)};EJS.Buffer.prototype={push:function(){this.line.push.apply(this.line,
arguments)},cr:function(){this.script.push(this.line.join(""),"\n");this.line=[]},close:function(){if(this.line.length>0){this.script.push(this.line.join(""));this.line=[]}this.post.length&&this.push.apply(this,this.post);this.script.push(";");return this.script.join("")}};EJS.options={type:"<",ext:".ejs"};EJS.Helpers=function(a,b){this._data=a;this._extras=b;j(this,b)};EJS.Helpers.prototype={plugin:function(){var a=g.makeArray(arguments),b=a.shift();return function(c){c=g(c);c[b].apply(c,a)}},view:function(a,
b,c){c=c||this._extras;b=b||this._data;return g.View(a,b,c)}};g.View.register({suffix:"ejs",script:function(a,b){return"jQuery.EJS(function(_CONTEXT,_VIEW) { "+(new EJS({text:b})).template.out+" })"},renderer:function(a,b){var c=new EJS({text:b,name:a});return function(e,d){return c.render.call(c,e,d)}}})});
;
steal.end();
steal.plugins("jquery").then(function(i){var x=function(a){return a.replace(/^\/\//,"").replace(/[\/\.]/g,"_")},y=1,e,o,p;isDeferred=function(a){return a&&i.isFunction(a.always)};getDeferreds=function(a){var b=[];if(isDeferred(a))return[a];else for(var c in a)isDeferred(a[c])&&b.push(a[c]);return b};usefulPart=function(a){return i.isArray(a)&&a.length===3&&a[1]==="success"?a[0]:a};e=i.View=function(a,b,c,d){if(typeof c==="function"){d=c;c=undefined}var f=getDeferreds(b);if(f.length){var g=i.Deferred();
f.push(p(a,true));i.when.apply(i,f).then(function(j){var k=i.makeArray(arguments),l=k.pop()[0];if(isDeferred(b))b=usefulPart(j);else for(var m in b)if(isDeferred(b[m]))b[m]=usefulPart(k.shift());k=l(b,c);g.resolve(k);d&&d(k)});return g.promise()}else{var h;f=typeof d==="function";g=p(a,f);if(f){h=g;g.done(function(j){d(j(b,c))})}else g.done(function(j){h=j(b,c)});return h}};o=function(a,b){if(!a.match(/[^\s]/))throw"$.View ERROR: There is no template or an empty template at "+b;};p=function(a,b){return i.ajax({url:a,
dataType:"view",async:b})};i.ajaxTransport("view",function(a,b){a=b.url;var c=a.match(/\.[\w\d]+$/),d,f,g,h=a,j,k=function(l){l=d.renderer(g,l);if(e.cache)e.cached[g]=l;return{view:l}};if(f=document.getElementById(a))c=f.type.match(/\/[\d\w]+$/)[0].replace(/^\//,".");if(!c){c=e.ext;h+=e.ext}g=x(h);if(h.match(/^\/\//))h=typeof steal==="undefined"?"/"+h.substr(2):steal.root.join(h.substr(2));d=e.types[c];return{send:function(l,m){if(e.cached[g])return m(200,"success",{view:e.cached[g]});else if(f)m(200,
"success",k(f.innerHTML));else j=i.ajax({async:b.async,url:h,dataType:"text",error:function(){o("",h);m(404)},success:function(s){o(s,h);m(200,"success",k(s))}})},abort:function(){j&&j.abort()}}});i.extend(e,{hookups:{},hookup:function(a){var b=++y;e.hookups[b]=a;return b},cached:{},cache:true,register:function(a){this.types["."+a.suffix]=a},types:{},ext:".ejs",registerScript:function(a,b,c){return"$.View.preload('"+b+"',"+e.types["."+a].script(b,c)+");"},preload:function(a,b){e.cached[a]=function(c,
d){return b.call(c,c,d)}}});var t,n,u,v,w,q;t=function(a){var b=i.fn[a];i.fn[a]=function(){var c=i.makeArray(arguments),d,f,g=this;if(u(c)){if(d=v(c)){f=c[d];c[d]=function(h){n.call(g,[h],b);f.call(g,h)};e.apply(e,c);return this}c=e.apply(e,c);if(isDeferred(c)){c.done(function(h){n.call(g,[h],b)});return this}else c=[c]}return n.call(this,c,b)}};n=function(a,b){var c;for(var d in e.hookups)break;if(d){c=e.hookups;e.hookups={};a[0]=i(a[0])}b=b.apply(this,a);d&&w(a[0],c);return b};u=function(a){var b=
typeof a[1];return typeof a[0]=="string"&&(b=="object"||b=="function")&&!a[1].nodeType&&!a[1].jquery};v=function(a){return typeof a[3]==="function"?3:typeof a[2]==="function"&&2};w=function(a,b){var c,d=0,f,g;a=a.filter(function(){return this.nodeType!=3});a=a.add("[data-view-id]",a);for(c=a.length;d<c;d++)if(a[d].getAttribute&&(f=a[d].getAttribute("data-view-id"))&&(g=b[f])){g(a[d],f);delete b[f];a[d].removeAttribute("data-view-id")}i.extend(e.hookups,b)};q=["prepend","append","after","before","text",
"html","replaceWith","val"];for(var r=0;r<q.length;r++)t(q[r])});
;
steal.end();
steal.plugins("jquery/lang").then(function(f){f.String.rsplit=function(a,e){for(var b=e.exec(a),c=[],d;b!==null;){d=b.index;if(d!==0){c.push(a.substring(0,d));a=a.slice(d)}c.push(b[0]);a=a.slice(b[0].length);b=e.exec(a)}a!==""&&c.push(a);return c}});
;
steal.end();
steal.plugins("jquery/controller","jquery/view").then(function(){jQuery.Controller.getFolder=function(){return jQuery.String.underscore(this.fullName.replace(/\./g,"/")).replace("/Controllers","")};var g=function(a,b,c){var d=a.fullName.replace(/\./g,"/"),e=d.indexOf("/Controllers/"+a.shortName)!=-1;d=jQuery.String.underscore(d.replace("/Controllers/"+a.shortName,""));a=a._shortName;var f=typeof b=="string"&&b.match(/\.[\w\d]+$/)||jQuery.View.ext;if(typeof b=="string"){if(b.substr(0,2)!="//")b="//"+
(new steal.File("views/"+(b.indexOf("/")!==-1?b:(e?a+"/":"")+b))).joinFrom(d)+f}else b||(b="//"+(new steal.File("views/"+(e?a+"/":"")+c.replace(/\.|#/g,"").replace(/ /g,"_"))).joinFrom(d)+f);return b},h=function(a){var b={};if(a)if(jQuery.isArray(a))for(var c=0;c<a.length;c++)jQuery.extend(b,a[c]);else jQuery.extend(b,a);else{if(this._default_helpers)b=this._default_helpers;a=window;c=this.Class.fullName.split(/\./);for(var d=0;d<c.length;d++){typeof a.Helpers=="object"&&jQuery.extend(b,a.Helpers);
a=a[c[d]]}typeof a.Helpers=="object"&&jQuery.extend(b,a.Helpers);this._default_helpers=b}return b};jQuery.Controller.prototype.view=function(a,b,c){if(typeof a!="string"&&!c){c=b;b=a;a=null}a=g(this.Class,a,this.called);b=b||this;c=h.call(this,c);return jQuery.View(a,b,c)}});
;
steal.end();
steal.plugins("jquery/class","jquery/lang").then(function(){var m=$.String.underscore,w=$.String.classize,q=$.isArray,r=$.makeArray,o=$.extend,j=$.each,x=/GET|POST|PUT|DELETE/i,k=function(a,b,c,e,f,d,g){g=g||"json";var h="";if(typeof a=="string"){var i=a.indexOf(" ");if(i>2&&i<7){h=a.substr(0,i);if(x.test(h))d=h;else g=h;h=a.substr(i+1)}else h=a}b=o({},b);a=$.String.sub(h,b,true);return $.ajax({url:a,data:b,success:c,error:e,type:d||"post",dataType:g,fixture:f})},s=function(a,b){var c=m(this.shortName),
e="-"+c+(a||"");return $.fixture&&$.fixture[e]?e:b||"//"+m(this.fullName).replace(/\.models\..*/,"").replace(/\./g,"/")+"/fixtures/"+c+(a||"")+".json"},y=function(a,b){a=a||{};var c=this.id;if(a[c]&&a[c]!==b){a["new"+$.String.capitalize(b)]=a[c];delete a[c]}a[c]=b;return a},t=function(a){return new (a||$.Model.List||Array)},n=function(a){return a[a.Class.id]},z=function(a){for(var b=[],c=0;c<a.length;c++)if(!a[c]["__u Nique"]){b.push(a[c]);a[c]["__u Nique"]=true}for(c=0;c<b.length;c++)delete b[c]["__u Nique"];
return b},u=function(a,b,c,e,f){var d=$.Deferred(),g=[a.attrs(),function(h){a[f||b+"d"](h);d.resolveWith(a,[a,h,b])},function(h){d.rejectWith(a,[h])}];b=="destroy"&&g.shift();b!=="create"&&g.unshift(n(a));d.then(c);d.fail(e);a.Class[b].apply(a.Class,g);return d.promise()},p=function(a){return typeof a==="object"&&a!==null&&a},l=function(a){return function(){$.fn[a].apply($([this]),arguments);return this}},v=l("bind");l=l("unbind");ajaxMethods={create:function(a){return function(b,c,e){return k(a,
b,c,e,"-restCreate")}},update:function(a){return function(b,c,e,f){return k(a,y.call(this,c,b),e,f,"-restUpdate","put")}},destroy:function(a){return function(b,c,e){var f={};f[this.id]=b;return k(a,f,c,e,"-restDestroy","delete")}},findAll:function(a){return function(b,c,e){return k(a||this.shortName+"s.json",b,c,e,s.call(this,"s"),"get","json "+this._shortName+".models")}},findOne:function(a){return function(b,c,e){return k(a,b,c,e,s.call(this),"get","json "+this._shortName+".model")}}};jQuery.Class("jQuery.Model",
{setup:function(a){var b=this;j(["attributes","associations","validations"],function(f,d){if(!b[d]||a[d]===b[d])b[d]={}});if(a.convert!=this.convert)this.convert=o(a.convert,this.convert);this._fullName=m(this.fullName.replace(/\./g,"_"));this._shortName=m(this.shortName);if(this.fullName.substr(0,7)!="jQuery."){if(this.listType)this.list=new this.listType([]);for(var c in ajaxMethods)if(typeof this[c]!=="function")this[c]=ajaxMethods[c](this[c]);c={};var e="* "+this._shortName+".model";c[e+"s"]=
this.callback("models");c[e]=this.callback("model");$.ajaxSetup({converters:c})}},attributes:{},model:function(a){if(!a)return null;return new this(p(a[this._shortName])||p(a.data)||p(a.attributes)||a)},models:function(a){if(!a)return null;var b=t(this.List),c=q(a),e=c?a:a.data,f=e.length,d=0;for(b._use_call=true;d<f;d++)b.push(this.model(e[d]));if(!c)for(var g in a)if(g!=="data")b[g]=a[g];return b},id:"id",addAttr:function(a,b){if(!this.associations[a]){this.attributes[a]||(this.attributes[a]=b);
return b}},_models:{},publish:function(a,b){window.OpenAjax&&OpenAjax.hub.publish(this._shortName+"."+a,b)},guessType:function(){return"string"},convert:{date:function(a){return typeof a==="string"?isNaN(Date.parse(a))?null:Date.parse(a):a},number:function(a){return parseFloat(a)},"boolean":function(a){return Boolean(a)}},bind:v,unbind:l},{setup:function(a){this._init=true;this.attrs(o({},this.Class.defaults,a));delete this._init},update:function(a,b,c){this.attrs(a);return this.save(b,c)},errors:function(a){if(a)a=
q(a)?a:r(arguments);var b={},c=this,e=function(d,g){j(g,function(h,i){if(h=i.call(c)){b.hasOwnProperty(d)||(b[d]=[]);b[d].push(h)}})};j(a||this.Class.validations||{},function(d,g){if(typeof d=="number"){d=g;g=c.Class.validations[d]}e(d,g||[])});for(var f in b)if(b.hasOwnProperty(f))return b;return null},attr:function(a,b,c,e){var f=w(a),d="get"+f;if(b!==undefined){this._setProperty(a,b,c,e,f);return this}return this[d]?this[d]():this[a]},bind:v,unbind:l,_setProperty:function(a,b,c,e,f){f="set"+f;
var d=this[a],g=this,h=function(i){e&&e.call(g,i);$(g).triggerHandler("error."+a,i)};this[f]&&(b=this[f](b,this.callback("_updateProperty",a,b,d,c,h),h))===undefined||this._updateProperty(a,b,d,c,h)},_updateProperty:function(a,b,c,e,f){var d=this.Class,g=d.attributes[a]||d.addAttr(a,d.guessType(b)),h=d.convert[g];g=null;b=this[a]=b===null?null:h?h.call(d,b):b;this._init||(g=this.errors(a));if(g)f(g);else{if(c!==b&&!this._init){$(this).triggerHandler(a,[b]);$(this).triggerHandler("updated.attr",[a,
b,c])}e&&e(this)}if(a===d.id&&b!==null&&d.list)if(c){if(c!=b){d.list.remove(c);d.list.push(this)}}else d.list.push(this)},attrs:function(a){var b;if(a){var c=this.Class.id;for(b in a)b!=c&&this.attr(b,a[b]);c in a&&this.attr(c,a[c])}else{a={};for(b in this.Class.attributes)if(this.Class.attributes.hasOwnProperty(b))a[b]=this.attr(b)}return a},isNew:function(){var a=n(this);return a===undefined||a===null},save:function(a,b){return u(this,this.isNew()?"create":"update",a,b)},destroy:function(a,b){return u(this,
"destroy",a,b,"destroyed")},identity:function(){var a=n(this);return this.Class._fullName+"_"+(this.Class.escapeIdentity?encodeURIComponent(a):a)},elements:function(a){return $("."+this.identity(),a)},publish:function(a,b){this.Class.publish(a,b||this)},hookup:function(a){var b=this.Class._shortName,c=$.data(a,"models")||$.data(a,"models",{});$(a).addClass(b+" "+this.identity());c[b]=this}});$.Model.wrapMany=$.Model.models;$.Model.wrap=$.Model.model;j(["created","updated","destroyed"],function(a,
b){$.Model.prototype[b]=function(c){b==="destroyed"&&this.Class.list&&this.Class.list.remove(n(this));c&&typeof c=="object"&&this.attrs(c.attrs?c.attrs():c);$(this).triggerHandler(b);this.publish(b,this);$([this.Class]).triggerHandler(b,this);return[this].concat(r(arguments))}});$.fn.models=function(){var a=[],b,c;this.each(function(){j($.data(this,"models")||{},function(e,f){b=b===undefined?f.Class.List||null:f.Class.List===b?b:null;a.push(f)})});c=t(b);c.push.apply(c,z(a));return c};$.fn.model=
function(a){if(a&&a instanceof $.Model){a.hookup(this[0]);return this}else return this.models.apply(this,arguments)[0]}});
;
steal.end();
steal.plugins("jquery/dom").then(function(c){c.ajaxPrefilter(function(a){if(c.fixture.on){x(a);if(a.fixture){if(typeof a.fixture==="string"&&c.fixture[a.fixture])a.fixture=c.fixture[a.fixture];if(typeof a.fixture=="string"){var b=a.fixture;if(/^\/\//.test(b))b=steal.root.join(a.fixture.substr(2));a.url=b;a.data=null;a.type="GET";if(!a.error)a.error=function(d,e,f){throw"fixtures.js Error "+e+" "+f;}}else a.dataTypes.splice(0,0,"fixture")}}});c.ajaxTransport("fixture",function(a,b){a.dataTypes.shift();
var d=a.dataTypes[0],e;return{send:function(f,q){e=setTimeout(function(){var i=a.fixture(b,a,f);if(!c.isArray(i)){var m=[{}];m[0][d]=i;i=m}typeof i[0]!="number"&&i.unshift(200,"success");if(!i[2]||!i[2][d]){m={};m[d]=i[2];i[2]=m}q.apply(null,i)},c.fixture.delay)},abort:function(){clearTimeout(e)}}});var v=/^(script|json|test|jsonp)$/,r=[],y=function(a,b,d){a=c.extend({},a);for(var e in b){if(e!=="fixture")if(b[e]!==a[e])return false;d&&delete a[e]}if(d)for(var f in a)return false;return true},w=function(a,
b){for(var d=0;d<r.length;d++)if(y(a,r[d],b))return d;return-1},x=function(a){var b=w(a);if(b>-1)a.fixture=r[b].fixture},t=function(a){var b=a.data.id;b===undefined&&a.url.replace(/\/(\d+)(\/|$)/g,function(d,e){b=e});if(b===undefined)b=a.url.replace(/\/(\w+)(\/|$)/g,function(d,e){if(e!="update")b=e});if(b===undefined)b=Math.round(Math.random()*1E3);return b};c.fixture=function(a,b){if(b!==undefined){if(typeof a=="string")a={url:a};var d=w(a,!!b);d>=-1&&r.splice(d,1);if(b!=null){a.fixture=b;r.push(a)}}};
c.extend(c.fixture,{"-restUpdate":function(a){return[{id:t(a)},{location:a.url+"/"+t(a)}]},"-restDestroy":function(){return{}},"-restCreate":function(a){var b=parseInt(Math.random()*1E5,10);return[{id:b},{location:a.url+"/"+b}]},make:function(a,b,d,e){if(typeof a==="string")a=[a+"s",a];for(var f=c.fixture["~"+a[0]]=[],q=function(g){for(var h=0;h<f.length;h++)if(g==f[h].id)return f[h]},i=0;i<b;i++){var m=d(i,f);if(!m.id)m.id=i;f.push(m)}c.fixture["-"+a[0]]=function(g){var h=f.slice(0);c.each((g.data.order||
[]).slice(0).reverse(),function(z,u){var k=u.split(" ");h=h.sort(function(n,o){return k[1].toUpperCase()!=="ASC"?n[k[0]]<o[k[0]]?1:n[k[0]]==o[k[0]]?0:-1:n[k[0]]<o[k[0]]?-1:n[k[0]]==o[k[0]]?0:1})});c.each((g.data.group||[]).slice(0).reverse(),function(z,u){var k=u.split(" ");h=h.sort(function(n,o){return n[k[0]]>o[k[0]]})});var j=parseInt(g.data.offset,10)||0,p=parseInt(g.data.limit,10)||b-j,l=0;for(var s in g.data){l=0;if(g.data[s]&&(s.indexOf("Id")!=-1||s.indexOf("_id")!=-1))for(;l<h.length;)if(g.data[s]!=
h[l][s])h.splice(l,1);else l++}if(e)for(l=0;l<h.length;)if(e(h[l],g))l++;else h.splice(l,1);return[{count:h.length,limit:g.data.limit,offset:g.data.offset,data:h.slice(j,j+p)}]};c.fixture["-"+a[1]]=function(g){return[q(g.data.id)]};c.fixture["-"+a[1]+"Update"]=function(g,h){var j=t(g);c.extend(q(j),g.data);return c.fixture["-restUpdate"](g,h)};c.fixture["-"+a[1]+"Destroy"]=function(g,h){for(var j=t(g),p=0;p<f.length;p++)if(f[p].id==j){f.splice(p,1);break}c.extend(q(j),g.data);return c.fixture["-restDestroy"](g,
h)};c.fixture["-"+a[1]+"Create"]=function(g,h){var j=d(f.length,f);c.extend(j,g.data);if(!j.id)j.id=f.length;f.push(j);return c.fixture["-restCreate"](g,h)}},xhr:function(a){return c.extend({},{abort:c.noop,getAllResponseHeaders:function(){return""},getResponseHeader:function(){return""},open:c.noop,overrideMimeType:c.noop,readyState:4,responseText:"",responseXML:null,send:c.noop,setRequestHeader:c.noop,status:200,statusText:"OK"},a)},on:true});c.fixture.delay=200;c.fixture["-handleFunction"]=function(a){if(typeof a.fixture===
"string"&&c.fixture[a.fixture])a.fixture=c.fixture[a.fixture];if(typeof a.fixture=="function"){setTimeout(function(){a.success&&a.success.apply(null,a.fixture(a,"success"));a.complete&&a.complete.apply(null,a.fixture(a,"complete"))},c.fixture.delay);return true}return false};c.get=function(a,b,d,e,f){if(jQuery.isFunction(b)){if(!v.test(e||"")){f=e;e=d}d=b;b=null}if(jQuery.isFunction(b)){f=e;e=d;d=b;b=null}return jQuery.ajax({type:"GET",url:a,data:b,success:d,dataType:e,fixture:f})};c.post=function(a,
b,d,e,f){if(jQuery.isFunction(b)){if(!v.test(e||"")){f=e;e=d}d=b;b={}}return jQuery.ajax({type:"POST",url:a,data:b,success:d,dataType:e,fixture:f})}});
;
steal.end();
steal.plugins("jquery");
;
steal.end();
steal.plugins("jquery/dom").then(function(g){var j=/radio|checkbox/i,k=/[^\[\]]+/g,l=/^[\-+]?[0-9]*\.?[0-9]+([eE][\-+]?[0-9]+)?$/,m=function(d){if(typeof d=="number")return true;if(typeof d!="string")return false;return d.match(l)};g.fn.extend({formParams:function(d){if(this[0].nodeName.toLowerCase()=="form"&&this[0].elements)return jQuery(jQuery.makeArray(this[0].elements)).getParams(d);return jQuery("input[name], textarea[name], select[name]",this[0]).getParams(d)},getParams:function(d){var h={},
b;d=d===undefined?true:d;this.each(function(){var c=this,i=c.type&&c.type.toLowerCase();if(!(i=="submit"||!c.name)){var a=c.name,e=g.data(c,"value")||g.fn.val.call([c]),f=j.test(c.type);a=a.match(k);c=!f||!!c.checked;if(d)if(m(e))e=parseFloat(e);else if(e==="true"||e==="false")e=Boolean(e);b=h;for(f=0;f<a.length-1;f++){b[a[f]]||(b[a[f]]={});b=b[a[f]]}a=a[a.length-1];if(a in b&&i==="checkbox"){g.isArray(b[a])||(b[a]=b[a]===undefined?[]:[b[a]]);c&&b[a].push(e)}else if(c||!b[a])b[a]=c?e:undefined}});
return h}})});
;
steal.end();
steal.plugins("jquery/lang/json").then(function(){jQuery.cookie=function(d,b,a){if(typeof b!="undefined"){a=a||{};if(b===null){b="";a.expires=-1}if(typeof b=="object"&&jQuery.toJSON)b=jQuery.toJSON(b);var c="";if(a.expires&&(typeof a.expires=="number"||a.expires.toUTCString)){if(typeof a.expires=="number"){c=new Date;c.setTime(c.getTime()+a.expires*24*60*60*1E3)}else c=a.expires;c="; expires="+c.toUTCString()}var e=a.path?"; path="+a.path:"",f=a.domain?"; domain="+a.domain:"";a=a.secure?"; secure":
"";document.cookie=[d,"=",encodeURIComponent(b),c,e,f,a].join("")}else{b=null;if(document.cookie&&document.cookie!=""){a=document.cookie.split(";");for(c=0;c<a.length;c++){e=jQuery.trim(a[c]);if(e.substring(0,d.length+1)==d+"="){b=decodeURIComponent(e.substring(d.length+1));break}}}if(jQuery.evalJSON&&b&&b.match(/^\s*\{/))try{b=jQuery.evalJSON(b)}catch(g){}return b}}});
;
steal.end();
steal.plugins("jquery").then(function(){(function(e){e.toJSON=function(a,c,b,h){if(typeof JSON=="object"&&JSON.stringify)return JSON.stringify(a,c,b);if(!h&&e.isFunction(c))a=c("",a);if(typeof b=="number")b="          ".substring(0,b);b=typeof b=="string"?b.substring(0,10):"";var f=typeof a;if(a===null)return"null";if(!(f=="undefined"||f=="function")){if(f=="number"||f=="boolean")return a+"";if(f=="string")return e.quoteString(a);if(f=="object"){if(typeof a.toJSON=="function")return e.toJSON(a.toJSON(),
c,b,true);if(a.constructor===Date){b=a.getUTCMonth()+1;if(b<10)b="0"+b;h=a.getUTCDate();if(h<10)h="0"+h;var i=a.getUTCFullYear(),g=a.getUTCHours();if(g<10)g="0"+g;var d=a.getUTCMinutes();if(d<10)d="0"+d;var j=a.getUTCSeconds();if(j<10)j="0"+j;a=a.getUTCMilliseconds();if(a<100)a="0"+a;if(a<10)a="0"+a;return'"'+i+"-"+b+"-"+h+"T"+g+":"+d+":"+j+"."+a+'Z"'}h=e.isFunction(c)?function(k,l){return c(k,l)}:function(k,l){return l};i=b?"\n":"";j=b?" ":"";if(a.constructor===Array){g=[];for(d=0;d<a.length;d++)g.push((e.toJSON(h(d,
a[d]),c,b,true)||"null").replace(/^/gm,b));return"["+i+g.join(","+i)+i+"]"}var n=[];if(e.isArray(c))g=e.map(c,function(k){return typeof k=="string"||typeof k=="number"?k+"":null});for(d in a){var m;f=typeof d;if(!(g&&e.inArray(d+"",g)==-1)){if(f=="number")f='"'+d+'"';else if(f=="string")f=e.quoteString(d);else continue;m=e.toJSON(h(d,a[d]),c,b,true);typeof m!="undefined"&&n.push((f+":"+j+m).replace(/^/gm,b))}}return"{"+i+n.join(","+i)+i+"}"}}};e.evalJSON=function(a){if(typeof JSON=="object"&&JSON.parse)return JSON.parse(a);
return eval("("+a+")")};e.secureEvalJSON=function(a){if(typeof JSON=="object"&&JSON.parse)return JSON.parse(a);var c=a;c=c.replace(/\\["\\\/bfnrtu]/g,"@");c=c.replace(/"[^"\\\n\r]*"|true|false|null|-?\d+(?:\.\d*)?(?:[eE][+\-]?\d+)?/g,"]");c=c.replace(/(?:^|:|,)(?:\s*\[)+/g,"");if(/^[\],:{}\s]*$/.test(c))return eval("("+a+")");else throw new SyntaxError("Error parsing JSON, source is not valid.");};e.quoteString=function(a){if(a.match(o))return'"'+a.replace(o,function(c){var b=p[c];if(typeof b==="string")return b;
b=c.charCodeAt();return"\\u00"+Math.floor(b/16).toString(16)+(b%16).toString(16)})+'"';return'"'+a+'"'};var o=/["\\\x00-\x1f\x7f-\x9f]/g,p={"\u0008":"\\b","\t":"\\t","\n":"\\n","\u000c":"\\f","\r":"\\r",'"':'\\"',"\\":"\\\\"}})(jQuery)});
;
steal.end();
(function(g){function t(b){var d={begin:0,end:0};if(b.setSelectionRange){d.begin=b.selectionStart;d.end=b.selectionEnd}else if(document.selection&&document.selection.createRange){b=document.selection.createRange();d.begin=0-b.duplicate().moveStart("character",-100000);d.end=d.begin+b.text.length}return d}function l(b,d){if(b.setSelectionRange){b.focus();b.setSelectionRange(d,d)}else if(b.createTextRange){b=b.createTextRange();b.collapse(true);b.moveEnd("character",d);b.moveStart("character",d);b.select()}}
var o={"9":"[0-9]",a:"[A-Za-z]","*":"[A-Za-z0-9]"};g.mask={addPlaceholder:function(b,d){o[b]=d}};g.fn.unmask=function(){return this.trigger("unmask")};g.fn.mask=function(b,d){d=g.extend({placeholder:"_",completed:null},d);for(var s="^",p=0;p<b.length;p++)s+=o[b.charAt(p)]||"\\"+b.charAt(p);s+="$";var x=new RegExp(s);return this.each(function(){function u(){m();h();setTimeout(function(){l(f[0],0)},0)}function v(a){var c=t(this);a=a.keyCode;q=a<16||a>16&&a<32||a>32&&a<41;if(c.begin-c.end!=0&&(!q||a==
8||a==46))r(c.begin,c.end);if(a==8)for(;c.begin-- >=0;){if(!n[c.begin]){i[c.begin]=d.placeholder;if(g.browser.opera){h(c.begin);l(this,c.begin+1)}else{h();l(this,c.begin)}return false}}else if(a==46){r(c.begin,c.begin+1);h();l(this,c.begin);return false}else if(a==27){r(0,b.length);h();l(this,0);return false}}function w(a){if(q)q=false;else{a=a||window.event;var c=a.charCode||a.keyCode||a.which,e=t(this),j=e.begin;if(a.ctrlKey||a.altKey)return true;else if(c>=41&&c<=122||c==32||c>186)for(;e.begin<
b.length;)if(a=o[b.charAt(e.begin)]){a=new RegExp(a);if(a=String.fromCharCode(c).match(a))i[e.begin]=String.fromCharCode(c);else return false;for(;++j<b.length;)if(!n[j])break;break}else{e.begin+=1;e.end=e.begin;j+=1}else return false;h();d.completed&&j>=i.length?d.completed.call(f):l(this,j);return false}}function r(a,c){for(a=a;a<c;a++)if(!n[a])i[a]=d.placeholder}function h(a){for(var c="",e=0;e<b.length;e++){c+=i[e];if(e==a)c+=d.placeholder}f.val(c);return c}function m(){for(var a=f.val(),c=0,
e=0;e<b.length;e++)if(!n[e])for(;c++<a.length;){var j=new RegExp(o[b.charAt(e)]);if(a.charAt(c-1).match(j)){i[e]=a.charAt(c-1);break}}if(!h().match(x)){f.val("");r(0,b.length)}}for(var f=g(this),i=new Array(b.length),n=new Array(b.length),k=0;k<b.length;k++){n[k]=o[b.charAt(k)]==null;i[k]=n[k]?b.charAt(k):d.placeholder}f.bind("focus",u);f.bind("blur",m);if(g.browser.msie)this.onpaste=function(){setTimeout(m,0)};else g.browser.mozilla&&this.addEventListener("input",m,false);var q=false;f.bind("keydown",
v);f.bind("keypress",w);f.one("unmask",function(){f.unbind("focus",u);f.unbind("blur",m);f.unbind("keydown",v);f.unbind("keypress",w);if(g.browser.msie)this.onpaste=null;else g.browser.mozilla&&this.removeEventListener("input",m,false)})})}})(jQuery);
;
steal.end();
var Hashtable=function(){function r(a){if(typeof a=="string")return a;else if(typeof a.hashCode==j){a=a.hashCode();return typeof a=="string"?a:r(a)}else if(typeof a.toString==j)return a.toString();else try{return String(a)}catch(b){return Object.prototype.toString.call(a)}}function B(a,b){return a.equals(b)}function C(a,b){return typeof b.equals==j?b.equals(a):a===b}function s(a){return function(b){if(b===null)throw new Error("null is not a valid "+a);else if(typeof b=="undefined")throw new Error(a+
" must not be undefined");}}function o(a,b,f,d){this[0]=a;this.entries=[];this.addEntry(b,f);if(d!==null)this.getEqualityFunction=function(){return d}}function p(a){return function(b){for(var f=this.entries.length,d,i=this.getEqualityFunction(b);f--;){d=this.entries[f];if(i(b,d[0]))switch(a){case t:return true;case u:return d;case v:return[f,d[1]]}}return false}}function w(a){return function(b){for(var f=b.length,d=0,i=this.entries.length;d<i;++d)b[f+d]=this.entries[d][a]}}function D(a,b){for(var f=
a.length,d;f--;){d=a[f];if(b===d[0])return f}return null}function l(a,b){return(a=a[b])&&a instanceof o?a:null}function x(a,b){var f=this,d=[],i={},m=typeof a==j?a:r,E=typeof b==j?b:null;this.put=function(c,e){n(c);y(e);var g=m(c),h,k=null;if(h=l(i,g))if(g=h.getEntryForKey(c)){k=g[1];g[1]=e}else h.addEntry(c,e);else{h=new o(g,c,e,E);d[d.length]=h;i[g]=h}return k};this.get=function(c){n(c);var e=m(c);if(e=l(i,e))if(c=e.getEntryForKey(c))return c[1];return null};this.containsKey=function(c){n(c);var e=
m(c);return(e=l(i,e))?e.containsKey(c):false};this.containsValue=function(c){y(c);for(var e=d.length;e--;)if(d[e].containsValue(c))return true;return false};this.clear=function(){d.length=0;i={}};this.isEmpty=function(){return!d.length};var q=function(c){return function(){for(var e=[],g=d.length;g--;)d[g][c](e);return e}};this.keys=q("keys");this.values=q("values");this.entries=q("getEntries");this.remove=function(c){n(c);var e=m(c),g=null,h=l(i,e);if(h){g=h.removeEntryForKey(c);if(g!==null)if(!h.entries.length){c=
D(d,e);z(d,c);delete i[e]}}return g};this.size=function(){for(var c=0,e=d.length;e--;)c+=d[e].entries.length;return c};this.each=function(c){for(var e=f.entries(),g=e.length,h;g--;){h=e[g];c(h[0],h[1])}};this.putAll=function(c,e){c=c.entries();for(var g,h,k,A=c.length,F=typeof e==j;A--;){g=c[A];h=g[0];g=g[1];if(F&&(k=f.get(h)))g=e(h,k,g);f.put(h,g)}};this.clone=function(){var c=new x(a,b);c.putAll(f);return c}}var j="function",z=typeof Array.prototype.splice==j?function(a,b){a.splice(b,1)}:function(a,
b){var f,d,i;if(b===a.length-1)a.length=b;else{f=a.slice(b+1);a.length=b;d=0;for(i=f.length;d<i;++d)a[b+d]=f[d]}},n=s("key"),y=s("value"),t=0,u=1,v=2;o.prototype={getEqualityFunction:function(a){return typeof a.equals==j?B:C},getEntryForKey:p(u),getEntryAndIndexForKey:p(v),removeEntryForKey:function(a){if(a=this.getEntryAndIndexForKey(a)){z(this.entries,a[0]);return a[1]}return null},addEntry:function(a,b){this.entries[this.entries.length]=[a,b]},keys:w(0),values:w(1),getEntries:function(a){for(var b=
a.length,f=0,d=this.entries.length;f<d;++f)a[b+f]=this.entries[f].slice(0)},containsKey:p(t),containsValue:function(a){for(var b=this.entries.length;b--;)if(a===this.entries[b][1])return true;return false}};return x}();
;
steal.end();
(function(d){function r(a,b,c){this.dec=a;this.group=b;this.neg=c}function s(){for(var a in p){localeGroup=p[a];for(var b=0;b<localeGroup.length;b++)n.put(localeGroup[b],a)}}function o(a){n.size()==0&&s();var b=".",c=",";if(a=n.get(a))if(a=t[a]){b=a[0];c=a[1]}return new r(b,c,"-")}var n=new Hashtable,t=[[".",","],[",","."],[","," "],[".","'"]],p=[["ae","au","ca","cn","eg","gb","hk","il","in","jp","sk","th","tw","us"],["at","br","de","dk","es","gr","it","nl","pt","tr","vn"],["cz","fi","fr","ru","se",
"pl"],["ch"]];d.fn.formatNumber=function(a,b,c){return this.each(function(){if(b==null)b=true;if(c==null)c=true;var e;e=d(this).is(":input")?new String(d(this).val()):new String(d(this).text());e=d.formatNumber(e,a);if(b)d(this).is(":input")?d(this).val(e):d(this).text(e);if(c)return e})};d.formatNumber=function(a,b){b=d.extend({},d.fn.formatNumber.defaults,b);o(b.locale.toLowerCase());for(var c="",e=false,h=0;h<b.format.length;h++)if("0#-,.".indexOf(b.format.charAt(h))==-1)c+=b.format.charAt(h);
else if(h==0&&b.format.charAt(h)=="-")e=true;else break;var k="";for(h=b.format.length-1;h>=0;h--)if("0#-,.".indexOf(b.format.charAt(h))==-1)k=b.format.charAt(h)+k;else break;b.format=b.format.substring(c.length);b.format=b.format.substring(0,b.format.length-k.length);a=new Number(a);return d._formatNumber(a,b,k,c,e)};d._formatNumber=function(a,b,c,e,h){b=d.extend({},d.fn.formatNumber.defaults,b);var k=o(b.locale.toLowerCase()),q=k.dec,u=k.group;k=k.neg;var m=false;if(isNaN(a))if(b.nanForceZero==
true){a=0;m=true}else return null;if(c=="%")a*=100;var j="";if(b.format.indexOf(".")>-1){var l=q,g=b.format.substring(b.format.lastIndexOf(".")+1);if(b.round==true)a=new Number(a.toFixed(g.length));else{a=a.toString();a=a.substring(0,a.lastIndexOf(".")+g.length+1);a=new Number(a)}var i=new String((a%1).toFixed(g.length));i=i.substring(i.lastIndexOf(".")+1);for(var f=0;f<g.length;f++)if(g.charAt(f)=="#"&&i.charAt(f)!="0")l+=i.charAt(f);else if(g.charAt(f)=="#"&&i.charAt(f)=="0")if(i.substring(f).match("[1-9]"))l+=
i.charAt(f);else break;else if(g.charAt(f)=="0")l+=i.charAt(f);j+=l}else a=Math.round(a);g=Math.floor(a);if(a<0)g=Math.ceil(a);f="";f=b.format.indexOf(".")==-1?b.format:b.format.substring(0,b.format.indexOf("."));l="";if(!(g==0&&f.substr(-1,1)=="#")||m){m=new String(Math.abs(g));g=9999;if(f.lastIndexOf(",")!=-1)g=f.length-f.lastIndexOf(",")-1;i=0;for(f=m.length-1;f>-1;f--){l=m.charAt(f)+l;i++;if(i==g&&f!=0){l=u+l;i=0}}}j=l+j;if(a<0&&h&&e.length>0)e=k+e;else if(a<0)j=k+j;if(!b.decimalSeparatorAlwaysShown)if(j.lastIndexOf(q)==
j.length-1)j=j.substring(0,j.length-1);return j=e+j+c};d.fn.parseNumber=function(a,b,c){if(b==null)b=true;if(c==null)c=true;var e;e=d(this).is(":input")?new String(d(this).val()):new String(d(this).text());if(a=d.parseNumber(e,a)){if(b)d(this).is(":input")?d(this).val(a.toString()):d(this).text(a.toString());if(c)return a}};d.parseNumber=function(a,b){b=d.extend({},d.fn.parseNumber.defaults,b);var c=o(b.locale.toLowerCase());b=c.dec;var e=c.group;for(c=c.neg;a.indexOf(e)>-1;)a=a.replace(e,"");a=a.replace(b,
".").replace(c,"-");b="";e=false;if(a.charAt(a.length-1)=="%")e=true;for(c=0;c<a.length;c++)if("1234567890.-".indexOf(a.charAt(c))>-1)b+=a.charAt(c);a=new Number(b);if(e){a/=100;a=a.toFixed(b.length-1)}return a};d.fn.parseNumber.defaults={locale:"us",decimalSeparatorAlwaysShown:false};d.fn.formatNumber.defaults={format:"#,###.00",locale:"us",decimalSeparatorAlwaysShown:false,nanForceZero:true,round:true}})(jQuery);
;
steal.end();
var dateFormat=function(){var k=/d{1,4}|m{1,4}|yy(?:yy)?|([HhMsTt])\1?|[LloSZ]|"[^"]*"|'[^']*'/g,l=/\b(?:[PMCEA][SDP]T|(?:Pacific|Mountain|Central|Eastern|Atlantic) (?:Standard|Daylight|Prevailing) Time|(?:GMT|UTC)(?:[-+]\d{4})?)\b/g,s=/[^-+\dA-Z]/g,d=function(a,c){a=String(a);for(c=c||2;a.length<c;)a="0"+a;return a};return function(a,c,h){var f=dateFormat;if(arguments.length==1&&Object.prototype.toString.call(a)=="[object String]"&&!/\d/.test(a)){c=a;a=undefined}a=a?new Date(a):new Date;if(isNaN(a))throw SyntaxError("invalid date");
c=String(f.masks[c]||c||f.masks["default"]);if(c.slice(0,4)=="UTC:"){c=c.slice(4);h=true}var b=h?"getUTC":"get",g=a[b+"Date"](),n=a[b+"Day"](),i=a[b+"Month"](),o=a[b+"FullYear"](),e=a[b+"Hours"](),p=a[b+"Minutes"](),q=a[b+"Seconds"]();b=a[b+"Milliseconds"]();var m=h?0:a.getTimezoneOffset(),r={d:g,dd:d(g),ddd:f.i18n.dayNames[n],dddd:f.i18n.dayNames[n+7],m:i+1,mm:d(i+1),mmm:f.i18n.monthNames[i],mmmm:f.i18n.monthNames[i+12],yy:String(o).slice(2),yyyy:o,h:e%12||12,hh:d(e%12||12),H:e,HH:d(e),M:p,MM:d(p),
s:q,ss:d(q),l:d(b,3),L:d(b>99?Math.round(b/10):b),t:e<12?"a":"p",tt:e<12?"am":"pm",T:e<12?"A":"P",TT:e<12?"AM":"PM",Z:h?"UTC":(String(a).match(l)||[""]).pop().replace(s,""),o:(m>0?"-":"+")+d(Math.floor(Math.abs(m)/60)*100+Math.abs(m)%60,4),S:["th","st","nd","rd"][g%10>3?0:(g%100-g%10!=10)*g%10]};return c.replace(k,function(j){return j in r?r[j]:j.slice(1,j.length-1)})}}();
dateFormat.masks={"default":"ddd mmm dd yyyy HH:MM:ss",shortDate:"m/d/yy",mediumDate:"mmm d, yyyy",longDate:"mmmm d, yyyy",fullDate:"dddd, mmmm d, yyyy",shortTime:"h:MM TT",mediumTime:"h:MM:ss TT",longTime:"h:MM:ss TT Z",isoDate:"yyyy-mm-dd",isoTime:"HH:MM:ss",isoDateTime:"yyyy-mm-dd'T'HH:MM:ss",isoUtcDateTime:"UTC:yyyy-mm-dd'T'HH:MM:ss'Z'"};
dateFormat.i18n={dayNames:["Sun","Mon","Tue","Wed","Thu","Fri","Sat","Sunday","Monday","Tuesday","Wednesday","Thursday","Friday","Saturday"],monthNames:["Jan","Feb","Mar","Apr","May","Jun","Jul","Aug","Sep","Oct","Nov","Dec","January","February","March","April","May","June","July","August","September","October","November","December"]};Date.prototype.format=function(k,l){return dateFormat(this,k,l)};
;
steal.end();
(function(c){c.extend(c.fn,{validate:function(a){if(this.length){var b=c.data(this[0],"validator");if(b)return b;this.attr("novalidate","novalidate");b=new c.validator(a,this[0]);c.data(this[0],"validator",b);if(b.settings.onsubmit){a=this.find("input, button");a.filter(".cancel").click(function(){b.cancelSubmit=true});b.settings.submitHandler&&a.filter(":submit").click(function(){b.submitButton=this});this.submit(function(d){function e(){if(b.settings.submitHandler){if(b.submitButton)var f=c("<input type='hidden'/>").attr("name",
b.submitButton.name).val(b.submitButton.value).appendTo(b.currentForm);b.settings.submitHandler.call(b,b.currentForm);b.submitButton&&f.remove();return false}return true}b.settings.debug&&d.preventDefault();if(b.cancelSubmit){b.cancelSubmit=false;return e()}if(b.form()){if(b.pendingRequest){b.formSubmitted=true;return false}return e()}else{b.focusInvalid();return false}})}return b}else a&&a.debug&&window.console&&console.warn("nothing selected, can't validate, returning nothing")},valid:function(){if(c(this[0]).is("form"))return this.validate().form();
else{var a=true,b=c(this[0].form).validate();this.each(function(){a&=b.element(this)});return a}},removeAttrs:function(a){var b={},d=this;c.each(a.split(/\s/),function(e,f){b[f]=d.attr(f);d.removeAttr(f)});return b},rules:function(a,b){var d=this[0];if(a){var e=c.data(d.form,"validator").settings,f=e.rules,g=c.validator.staticRules(d);switch(a){case "add":c.extend(g,c.validator.normalizeRule(b));f[d.name]=g;if(b.messages)e.messages[d.name]=c.extend(e.messages[d.name],b.messages);break;case "remove":if(!b){delete f[d.name];
return g}var h={};c.each(b.split(/\s/),function(j,i){h[i]=g[i];delete g[i]});return h}}a=c.validator.normalizeRules(c.extend({},c.validator.metadataRules(d),c.validator.classRules(d),c.validator.attributeRules(d),c.validator.staticRules(d)),d);if(a.required){b=a.required;delete a.required;a=c.extend({required:b},a)}return a}});c.extend(c.expr[":"],{blank:function(a){return!c.trim(""+a.value)},filled:function(a){return!!c.trim(""+a.value)},unchecked:function(a){return!a.checked}});c.validator=function(a,
b){this.settings=c.extend(true,{},c.validator.defaults,a);this.currentForm=b;this.init()};c.validator.format=function(a,b){if(arguments.length==1)return function(){var d=c.makeArray(arguments);d.unshift(a);return c.validator.format.apply(this,d)};if(arguments.length>2&&b.constructor!=Array)b=c.makeArray(arguments).slice(1);if(b.constructor!=Array)b=[b];c.each(b,function(d,e){a=a.replace(new RegExp("\\{"+d+"\\}","g"),e)});return a};c.extend(c.validator,{defaults:{messages:{},groups:{},rules:{},errorClass:"error",
validClass:"valid",errorElement:"label",focusInvalid:true,errorContainer:c([]),errorLabelContainer:c([]),onsubmit:true,ignore:":hidden",ignoreTitle:false,onfocusin:function(a){this.lastActive=a;if(this.settings.focusCleanup&&!this.blockFocusCleanup){this.settings.unhighlight&&this.settings.unhighlight.call(this,a,this.settings.errorClass,this.settings.validClass);this.addWrapper(this.errorsFor(a)).hide()}},onfocusout:function(a){if(!this.checkable(a)&&(a.name in this.submitted||!this.optional(a)))this.element(a)},
onkeyup:function(a){if(a.name in this.submitted||a==this.lastElement)this.element(a)},onclick:function(a){if(a.name in this.submitted)this.element(a);else a.parentNode.name in this.submitted&&this.element(a.parentNode)},highlight:function(a,b,d){a.type==="radio"?this.findByName(a.name).addClass(b).removeClass(d):c(a).addClass(b).removeClass(d)},unhighlight:function(a,b,d){a.type==="radio"?this.findByName(a.name).removeClass(b).addClass(d):c(a).removeClass(b).addClass(d)}},setDefaults:function(a){c.extend(c.validator.defaults,
a)},messages:{required:"This field is required.",remote:"Please fix this field.",email:"Please enter a valid email address.",url:"Please enter a valid URL.",date:"Please enter a valid date.",dateISO:"Please enter a valid date (ISO).",number:"Please enter a valid number.",digits:"Please enter only digits.",creditcard:"Please enter a valid credit card number.",equalTo:"Please enter the same value again.",accept:"Please enter a value with a valid extension.",maxlength:c.validator.format("Please enter no more than {0} characters."),
minlength:c.validator.format("Please enter at least {0} characters."),rangelength:c.validator.format("Please enter a value between {0} and {1} characters long."),range:c.validator.format("Please enter a value between {0} and {1}."),max:c.validator.format("Please enter a value less than or equal to {0}."),min:c.validator.format("Please enter a value greater than or equal to {0}.")},autoCreateRanges:false,prototype:{init:function(){function a(e){var f=c.data(this[0].form,"validator"),g="on"+e.type.replace(/^validate/,
"");f.settings[g]&&f.settings[g].call(f,this[0],e)}this.labelContainer=c(this.settings.errorLabelContainer);this.errorContext=this.labelContainer.length&&this.labelContainer||c(this.currentForm);this.containers=c(this.settings.errorContainer).add(this.settings.errorLabelContainer);this.submitted={};this.valueCache={};this.pendingRequest=0;this.pending={};this.invalid={};this.reset();var b=this.groups={};c.each(this.settings.groups,function(e,f){c.each(f.split(/\s/),function(g,h){b[h]=e})});var d=
this.settings.rules;c.each(d,function(e,f){d[e]=c.validator.normalizeRule(f)});c(this.currentForm).validateDelegate("[type='text'], [type='password'], [type='file'], select, textarea, [type='number'], [type='search'] ,[type='tel'], [type='url'], [type='email'], [type='datetime'], [type='date'], [type='month'], [type='week'], [type='time'], [type='datetime-local'], [type='range'], [type='color'] ","focusin focusout keyup",a).validateDelegate("[type='radio'], [type='checkbox'], select, option","click",
a);this.settings.invalidHandler&&c(this.currentForm).bind("invalid-form.validate",this.settings.invalidHandler)},form:function(){this.checkForm();c.extend(this.submitted,this.errorMap);this.invalid=c.extend({},this.errorMap);this.valid()||c(this.currentForm).triggerHandler("invalid-form",[this]);this.showErrors();return this.valid()},checkForm:function(){this.prepareForm();for(var a=0,b=this.currentElements=this.elements();b[a];a++)this.check(b[a]);return this.valid()},element:function(a){this.lastElement=
a=this.validationTargetFor(this.clean(a));this.prepareElement(a);this.currentElements=c(a);var b=this.check(a);if(b)delete this.invalid[a.name];else this.invalid[a.name]=true;if(!this.numberOfInvalids())this.toHide=this.toHide.add(this.containers);this.showErrors();return b},showErrors:function(a){if(a){c.extend(this.errorMap,a);this.errorList=[];for(var b in a)this.errorList.push({message:a[b],element:this.findByName(b)[0]});this.successList=c.grep(this.successList,function(d){return!(d.name in a)})}this.settings.showErrors?
this.settings.showErrors.call(this,this.errorMap,this.errorList):this.defaultShowErrors()},resetForm:function(){c.fn.resetForm&&c(this.currentForm).resetForm();this.submitted={};this.lastElement=null;this.prepareForm();this.hideErrors();this.elements().removeClass(this.settings.errorClass)},numberOfInvalids:function(){return this.objectLength(this.invalid)},objectLength:function(a){var b=0;for(var d in a)b++;return b},hideErrors:function(){this.addWrapper(this.toHide).hide()},valid:function(){return this.size()==
0},size:function(){return this.errorList.length},focusInvalid:function(){if(this.settings.focusInvalid)try{c(this.findLastActive()||this.errorList.length&&this.errorList[0].element||[]).filter(":visible").focus().trigger("focusin")}catch(a){}},findLastActive:function(){var a=this.lastActive;return a&&c.grep(this.errorList,function(b){return b.element.name==a.name}).length==1&&a},elements:function(){var a=this,b={};return c(this.currentForm).find("input, select, textarea").not(":submit, :reset, :image, [disabled]").not(this.settings.ignore).filter(function(){!this.name&&
a.settings.debug&&window.console&&console.error("%o has no name assigned",this);if(this.name in b||!a.objectLength(c(this).rules()))return false;return b[this.name]=true})},clean:function(a){return c(a)[0]},errors:function(){return c(this.settings.errorElement+"."+this.settings.errorClass,this.errorContext)},reset:function(){this.successList=[];this.errorList=[];this.errorMap={};this.toShow=c([]);this.toHide=c([]);this.currentElements=c([])},prepareForm:function(){this.reset();this.toHide=this.errors().add(this.containers)},
prepareElement:function(a){this.reset();this.toHide=this.errorsFor(a)},check:function(a){a=this.validationTargetFor(this.clean(a));var b=c(a).rules(),d=false;for(var e in b){var f={method:e,parameters:b[e]};try{var g=c.validator.methods[e].call(this,a.value.replace(/\r/g,""),a,f.parameters);if(g=="dependency-mismatch")d=true;else{d=false;if(g=="pending"){this.toHide=this.toHide.not(this.errorsFor(a));return}if(!g){this.formatAndAdd(a,f);return false}}}catch(h){this.settings.debug&&window.console&&
console.log("exception occured when checking element "+a.id+", check the '"+f.method+"' method",h);throw h;}}if(!d){this.objectLength(b)&&this.successList.push(a);return true}},customMetaMessage:function(a,b){if(c.metadata)return(a=this.settings.meta?c(a).metadata()[this.settings.meta]:c(a).metadata())&&a.messages&&a.messages[b]},customMessage:function(a,b){return(a=this.settings.messages[a])&&(a.constructor==String?a:a[b])},findDefined:function(){for(var a=0;a<arguments.length;a++)if(arguments[a]!==
undefined)return arguments[a]},defaultMessage:function(a,b){return this.findDefined(this.customMessage(a.name,b),this.customMetaMessage(a,b),!this.settings.ignoreTitle&&a.title||undefined,c.validator.messages[b],"<strong>Warning: No message defined for "+a.name+"</strong>")},formatAndAdd:function(a,b){var d=this.defaultMessage(a,b.method),e=/\$?\{(\d+)\}/g;if(typeof d=="function")d=d.call(this,b.parameters,a);else if(e.test(d))d=jQuery.format(d.replace(e,"{$1}"),b.parameters);this.errorList.push({message:d,
element:a});this.errorMap[a.name]=d;this.submitted[a.name]=d},addWrapper:function(a){if(this.settings.wrapper)a=a.add(a.parent(this.settings.wrapper));return a},defaultShowErrors:function(){for(var a=0;this.errorList[a];a++){var b=this.errorList[a];this.settings.highlight&&this.settings.highlight.call(this,b.element,this.settings.errorClass,this.settings.validClass);this.showLabel(b.element,b.message)}if(this.errorList.length)this.toShow=this.toShow.add(this.containers);if(this.settings.success)for(a=
0;this.successList[a];a++)this.showLabel(this.successList[a]);if(this.settings.unhighlight){a=0;for(b=this.validElements();b[a];a++)this.settings.unhighlight.call(this,b[a],this.settings.errorClass,this.settings.validClass)}this.toHide=this.toHide.not(this.toShow);this.hideErrors();this.addWrapper(this.toShow).show()},validElements:function(){return this.currentElements.not(this.invalidElements())},invalidElements:function(){return c(this.errorList).map(function(){return this.element})},showLabel:function(a,
b){var d=this.errorsFor(a);if(d.length){d.removeClass(this.settings.validClass).addClass(this.settings.errorClass);d.attr("generated")&&d.html(b)}else{d=c("<"+this.settings.errorElement+"/>").attr({"for":this.idOrName(a),generated:true}).addClass(this.settings.errorClass).html(b||"");if(this.settings.wrapper)d=d.hide().show().wrap("<"+this.settings.wrapper+"/>").parent();this.labelContainer.append(d).length||(this.settings.errorPlacement?this.settings.errorPlacement(d,c(a)):d.insertAfter(a))}if(!b&&
this.settings.success){d.text("");typeof this.settings.success=="string"?d.addClass(this.settings.success):this.settings.success(d)}this.toShow=this.toShow.add(d)},errorsFor:function(a){var b=this.idOrName(a);return this.errors().filter(function(){return c(this).attr("for")==b})},idOrName:function(a){return this.groups[a.name]||(this.checkable(a)?a.name:a.id||a.name)},validationTargetFor:function(a){if(this.checkable(a))a=this.findByName(a.name).not(this.settings.ignore)[0];return a},checkable:function(a){return/radio|checkbox/i.test(a.type)},
findByName:function(a){var b=this.currentForm;return c(document.getElementsByName(a)).map(function(d,e){return e.form==b&&e.name==a&&e||null})},getLength:function(a,b){switch(b.nodeName.toLowerCase()){case "select":return c("option:selected",b).length;case "input":if(this.checkable(b))return this.findByName(b.name).filter(":checked").length}return a.length},depend:function(a,b){return this.dependTypes[typeof a]?this.dependTypes[typeof a](a,b):true},dependTypes:{"boolean":function(a){return a},string:function(a,
b){return!!c(a,b.form).length},"function":function(a,b){return a(b)}},optional:function(a){return!c.validator.methods.required.call(this,c.trim(a.value),a)&&"dependency-mismatch"},startRequest:function(a){if(!this.pending[a.name]){this.pendingRequest++;this.pending[a.name]=true}},stopRequest:function(a,b){this.pendingRequest--;if(this.pendingRequest<0)this.pendingRequest=0;delete this.pending[a.name];if(b&&this.pendingRequest==0&&this.formSubmitted&&this.form()){c(this.currentForm).submit();this.formSubmitted=
false}else if(!b&&this.pendingRequest==0&&this.formSubmitted){c(this.currentForm).triggerHandler("invalid-form",[this]);this.formSubmitted=false}},previousValue:function(a){return c.data(a,"previousValue")||c.data(a,"previousValue",{old:null,valid:true,message:this.defaultMessage(a,"remote")})}},classRuleSettings:{required:{required:true},email:{email:true},url:{url:true},date:{date:true},dateISO:{dateISO:true},dateDE:{dateDE:true},number:{number:true},numberDE:{numberDE:true},digits:{digits:true},
creditcard:{creditcard:true}},addClassRules:function(a,b){a.constructor==String?(this.classRuleSettings[a]=b):c.extend(this.classRuleSettings,a)},classRules:function(a){var b={};(a=c(a).attr("class"))&&c.each(a.split(" "),function(){this in c.validator.classRuleSettings&&c.extend(b,c.validator.classRuleSettings[this])});return b},attributeRules:function(a){var b={};a=c(a);for(var d in c.validator.methods){var e;if(e=d==="required"&&typeof c.fn.prop==="function"?a.prop(d):a.attr(d))b[d]=e;else if(a[0].getAttribute("type")===
d)b[d]=true}b.maxlength&&/-1|2147483647|524288/.test(b.maxlength)&&delete b.maxlength;return b},metadataRules:function(a){if(!c.metadata)return{};var b=c.data(a.form,"validator").settings.meta;return b?c(a).metadata()[b]:c(a).metadata()},staticRules:function(a){var b={},d=c.data(a.form,"validator");if(d.settings.rules)b=c.validator.normalizeRule(d.settings.rules[a.name])||{};return b},normalizeRules:function(a,b){c.each(a,function(d,e){if(e===false)delete a[d];else if(e.param||e.depends){var f=true;
switch(typeof e.depends){case "string":f=!!c(e.depends,b.form).length;break;case "function":f=e.depends.call(b,b);break}if(f)a[d]=e.param!==undefined?e.param:true;else delete a[d]}});c.each(a,function(d,e){a[d]=c.isFunction(e)?e(b):e});c.each(["minlength","maxlength","min","max"],function(){if(a[this])a[this]=Number(a[this])});c.each(["rangelength","range"],function(){if(a[this])a[this]=[Number(a[this][0]),Number(a[this][1])]});if(c.validator.autoCreateRanges){if(a.min&&a.max){a.range=[a.min,a.max];
delete a.min;delete a.max}if(a.minlength&&a.maxlength){a.rangelength=[a.minlength,a.maxlength];delete a.minlength;delete a.maxlength}}a.messages&&delete a.messages;return a},normalizeRule:function(a){if(typeof a=="string"){var b={};c.each(a.split(/\s/),function(){b[this]=true});a=b}return a},addMethod:function(a,b,d){c.validator.methods[a]=b;c.validator.messages[a]=d!=undefined?d:c.validator.messages[a];b.length<3&&c.validator.addClassRules(a,c.validator.normalizeRule(a))},methods:{required:function(a,
b,d){if(!this.depend(d,b))return"dependency-mismatch";switch(b.nodeName.toLowerCase()){case "select":return(a=c(b).val())&&a.length>0;case "input":if(this.checkable(b))return this.getLength(a,b)>0;default:return c.trim(a).length>0}},remote:function(a,b,d){if(this.optional(b))return"dependency-mismatch";var e=this.previousValue(b);this.settings.messages[b.name]||(this.settings.messages[b.name]={});e.originalMessage=this.settings.messages[b.name].remote;this.settings.messages[b.name].remote=e.message;
d=typeof d=="string"&&{url:d}||d;if(this.pending[b.name])return"pending";if(e.old===a)return e.valid;e.old=a;var f=this;this.startRequest(b);var g={};g[b.name]=a;c.ajax(c.extend(true,{url:d,mode:"abort",port:"validate"+b.name,dataType:"json",data:g,success:function(h){f.settings.messages[b.name].remote=e.originalMessage;var j=h===true;if(j){var i=f.formSubmitted;f.prepareElement(b);f.formSubmitted=i;f.successList.push(b);f.showErrors()}else{i={};h=h||f.defaultMessage(b,"remote");i[b.name]=e.message=
c.isFunction(h)?h(a):h;f.showErrors(i)}e.valid=j;f.stopRequest(b,j)}},d));return"pending"},minlength:function(a,b,d){return this.optional(b)||this.getLength(c.trim(a),b)>=d},maxlength:function(a,b,d){return this.optional(b)||this.getLength(c.trim(a),b)<=d},rangelength:function(a,b,d){a=this.getLength(c.trim(a),b);return this.optional(b)||a>=d[0]&&a<=d[1]},min:function(a,b,d){return this.optional(b)||a>=d},max:function(a,b,d){return this.optional(b)||a<=d},range:function(a,b,d){return this.optional(b)||
a>=d[0]&&a<=d[1]},email:function(a,b){return this.optional(b)||/^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))$/i.test(a)},
url:function(a,b){return this.optional(b)||/^(https?|ftp):\/\/(((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:)*@)?(((\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5]))|((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?)(:\d*)?)(\/((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)+(\/(([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)*)*)?)?(\?((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)|[\uE000-\uF8FF]|\/|\?)*)?(\#((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)|\/|\?)*)?$/i.test(a)},
date:function(a,b){return this.optional(b)||!/Invalid|NaN/.test(new Date(a))},dateISO:function(a,b){return this.optional(b)||/^\d{4}[\/-]\d{1,2}[\/-]\d{1,2}$/.test(a)},number:function(a,b){return this.optional(b)||/^-?(?:\d+|\d{1,3}(?:,\d{3})+)(?:\.\d+)?$/.test(a)},digits:function(a,b){return this.optional(b)||/^\d+$/.test(a)},creditcard:function(a,b){if(this.optional(b))return"dependency-mismatch";if(/[^0-9 -]+/.test(a))return false;var d=b=0,e=false;a=a.replace(/\D/g,"");for(var f=a.length-1;f>=
0;f--){d=a.charAt(f);d=parseInt(d,10);if(e)if((d*=2)>9)d-=9;b+=d;e=!e}return b%10==0},accept:function(a,b,d){d=typeof d=="string"?d.replace(/,/g,"|"):"png|jpe?g|gif";return this.optional(b)||a.match(new RegExp(".("+d+")$","i"))},equalTo:function(a,b,d){d=c(d).unbind(".validate-equalTo").bind("blur.validate-equalTo",function(){c(b).valid()});return a==d.val()}}});c.format=c.validator.format})(jQuery);
(function(c){var a={};if(c.ajaxPrefilter)c.ajaxPrefilter(function(d,e,f){e=d.port;if(d.mode=="abort"){a[e]&&a[e].abort();a[e]=f}});else{var b=c.ajax;c.ajax=function(d){var e=("port"in d?d:c.ajaxSettings).port;if(("mode"in d?d:c.ajaxSettings).mode=="abort"){a[e]&&a[e].abort();return a[e]=b.apply(this,arguments)}return b.apply(this,arguments)}}})(jQuery);
(function(c){!jQuery.event.special.focusin&&!jQuery.event.special.focusout&&document.addEventListener&&c.each({focus:"focusin",blur:"focusout"},function(a,b){function d(e){e=c.event.fix(e);e.type=b;return c.event.handle.call(this,e)}c.event.special[b]={setup:function(){this.addEventListener(a,d,true)},teardown:function(){this.removeEventListener(a,d,true)},handler:function(e){arguments[0]=c.event.fix(e);arguments[0].type=b;return c.event.handle.apply(this,arguments)}}});c.extend(c.fn,{validateDelegate:function(a,
b,d){return this.bind(b,function(e){var f=c(e.target);if(f.is(a))return d.apply(f,arguments)})}})})(jQuery);
;
steal.end();
(function(){function a(b){return b.replace(/<.[^<>]*?>/g," ").replace(/&nbsp;|&#160;/gi," ").replace(/[0-9.(),;:!?%#$'"_+=\/-]*/g,"")}jQuery.validator.addMethod("maxWords",function(b,c,d){return this.optional(c)||a(b).match(/\b\w+\b/g).length<d},jQuery.validator.format("Please enter {0} words or less."));jQuery.validator.addMethod("minWords",function(b,c,d){return this.optional(c)||a(b).match(/\b\w+\b/g).length>=d},jQuery.validator.format("Please enter at least {0} words."));jQuery.validator.addMethod("rangeWords",
function(b,c,d){return this.optional(c)||a(b).match(/\b\w+\b/g).length>=d[0]&&b.match(/bw+b/g).length<d[1]},jQuery.validator.format("Please enter between {0} and {1} words."))})();jQuery.validator.addMethod("letterswithbasicpunc",function(a,b){return this.optional(b)||/^[a-z-.,()'\"\s]+$/i.test(a)},"Letters or punctuation only please");jQuery.validator.addMethod("alphanumeric",function(a,b){return this.optional(b)||/^\w+$/i.test(a)},"Letters, numbers, spaces or underscores only please");
jQuery.validator.addMethod("lettersonly",function(a,b){return this.optional(b)||/^[a-z]+$/i.test(a)},"Letters only please");jQuery.validator.addMethod("nowhitespace",function(a,b){return this.optional(b)||/^\S+$/i.test(a)},"No white space please");jQuery.validator.addMethod("ziprange",function(a,b){return this.optional(b)||/^90[2-5]\d\{2}-\d{4}$/.test(a)},"Your ZIP-code must be in the range 902xx-xxxx to 905-xx-xxxx");
jQuery.validator.addMethod("integer",function(a,b){return this.optional(b)||/^-?\d+$/.test(a)},"A positive or negative non-decimal number please");
jQuery.validator.addMethod("vinUS",function(a){if(a.length!=17)return false;var b,c,d,e,f,g=["A","B","C","D","E","F","G","H","J","K","L","M","N","P","R","S","T","U","V","W","X","Y","Z"],i=[1,2,3,4,5,6,7,8,1,2,3,4,5,7,9,2,3,4,5,6,7,8,9],j=[8,7,6,5,4,3,2,10,0,9,8,7,6,5,4,3,2],h=0;for(b=0;b<17;b++){e=j[b];d=a.slice(b,b+1);if(b==8)f=d;if(isNaN(d))for(c=0;c<g.length;c++){if(d.toUpperCase()===g[c]){d=i[c];d*=e;if(isNaN(f)&&c==8)f=g[c];break}}else d*=e;h+=d}a=h%11;if(a==10)a="X";if(a==f)return true;return false},
"The specified vehicle identification number (VIN) is invalid.");jQuery.validator.addMethod("dateITA",function(a,b){var c=false;if(/^\d{1,2}\/\d{1,2}\/\d{4}$/.test(a)){var d=a.split("/");a=parseInt(d[0],10);c=parseInt(d[1],10);d=parseInt(d[2],10);var e=new Date(d,c-1,a);c=e.getFullYear()==d&&e.getMonth()==c-1&&e.getDate()==a?true:false}else c=false;return this.optional(b)||c},"Please enter a correct date");
jQuery.validator.addMethod("dateNL",function(a,b){return this.optional(b)||/^\d\d?[\.\/-]\d\d?[\.\/-]\d\d\d?\d?$/.test(a)},"Vul hier een geldige datum in.");jQuery.validator.addMethod("time",function(a,b){return this.optional(b)||/^([01]\d|2[0-3])(:[0-5]\d){0,2}$/.test(a)},"Please enter a valid time, between 00:00 and 23:59");jQuery.validator.addMethod("time12h",function(a,b){return this.optional(b)||/^((0?[1-9]|1[012])(:[0-5]\d){0,2}(\ [AP]M))$/i.test(a)},"Please enter a valid time, between 00:00 am and 12:00 pm");
jQuery.validator.addMethod("phoneUS",function(a,b){a=a.replace(/\s+/g,"");return this.optional(b)||a.length>9&&a.match(/^(1-?)?(\([2-9]\d{2}\)|[2-9]\d{2})-?[2-9]\d{2}-?\d{4}$/)},"Please specify a valid phone number");jQuery.validator.addMethod("phoneUK",function(a,b){return this.optional(b)||a.length>9&&a.match(/^(\(?(0|\+44)[1-9]{1}\d{1,4}?\)?\s?\d{3,4}\s?\d{3,4})$/)},"Please specify a valid phone number");
jQuery.validator.addMethod("mobileUK",function(a,b){return this.optional(b)||a.length>9&&a.match(/^((0|\+44)7(5|6|7|8|9){1}\d{2}\s?\d{6})$/)},"Please specify a valid mobile number");jQuery.validator.addMethod("strippedminlength",function(a,b,c){return jQuery(a).text().length>=c},jQuery.validator.format("Please enter at least {0} characters"));
jQuery.validator.addMethod("email2",function(a,b){return this.optional(b)||/^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)*(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?$/i.test(a)},jQuery.validator.messages.email);
jQuery.validator.addMethod("url2",function(a,b){return this.optional(b)||/^(https?|ftp):\/\/(((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:)*@)?(((\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5]))|((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)*(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?)(:\d*)?)(\/((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)+(\/(([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)*)*)?)?(\?((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)|[\uE000-\uF8FF]|\/|\?)*)?(\#((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)|\/|\?)*)?$/i.test(a)},
jQuery.validator.messages.url);
jQuery.validator.addMethod("creditcardtypes",function(a,b,c){if(/[^0-9-]+/.test(a))return false;a=a.replace(/\D/g,"");b=0;if(c.mastercard)b|=1;if(c.visa)b|=2;if(c.amex)b|=4;if(c.dinersclub)b|=8;if(c.enroute)b|=16;if(c.discover)b|=32;if(c.jcb)b|=64;if(c.unknown)b|=128;if(c.all)b=255;if(b&1&&/^(51|52|53|54|55)/.test(a))return a.length==16;if(b&2&&/^(4)/.test(a))return a.length==16;if(b&4&&/^(34|37)/.test(a))return a.length==15;if(b&8&&/^(300|301|302|303|304|305|36|38)/.test(a))return a.length==14;if(b&
16&&/^(2014|2149)/.test(a))return a.length==15;if(b&32&&/^(6011)/.test(a))return a.length==16;if(b&64&&/^(3)/.test(a))return a.length==16;if(b&64&&/^(2131|1800)/.test(a))return a.length==15;if(b&128)return true;return false},"Please enter a valid credit card number.");
jQuery.validator.addMethod("ipv4",function(a,b){return this.optional(b)||/^(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$/i.test(a)},"Please enter a valid IP v4 address.");
jQuery.validator.addMethod("ipv6",function(a,b){return this.optional(b)||/^((([0-9A-Fa-f]{1,4}:){7}[0-9A-Fa-f]{1,4})|(([0-9A-Fa-f]{1,4}:){6}:[0-9A-Fa-f]{1,4})|(([0-9A-Fa-f]{1,4}:){5}:([0-9A-Fa-f]{1,4}:)?[0-9A-Fa-f]{1,4})|(([0-9A-Fa-f]{1,4}:){4}:([0-9A-Fa-f]{1,4}:){0,2}[0-9A-Fa-f]{1,4})|(([0-9A-Fa-f]{1,4}:){3}:([0-9A-Fa-f]{1,4}:){0,3}[0-9A-Fa-f]{1,4})|(([0-9A-Fa-f]{1,4}:){2}:([0-9A-Fa-f]{1,4}:){0,4}[0-9A-Fa-f]{1,4})|(([0-9A-Fa-f]{1,4}:){6}((\b((25[0-5])|(1\d{2})|(2[0-4]\d)|(\d{1,2}))\b)\.){3}(\b((25[0-5])|(1\d{2})|(2[0-4]\d)|(\d{1,2}))\b))|(([0-9A-Fa-f]{1,4}:){0,5}:((\b((25[0-5])|(1\d{2})|(2[0-4]\d)|(\d{1,2}))\b)\.){3}(\b((25[0-5])|(1\d{2})|(2[0-4]\d)|(\d{1,2}))\b))|(::([0-9A-Fa-f]{1,4}:){0,5}((\b((25[0-5])|(1\d{2})|(2[0-4]\d)|(\d{1,2}))\b)\.){3}(\b((25[0-5])|(1\d{2})|(2[0-4]\d)|(\d{1,2}))\b))|([0-9A-Fa-f]{1,4}::([0-9A-Fa-f]{1,4}:){0,5}[0-9A-Fa-f]{1,4})|(::([0-9A-Fa-f]{1,4}:){0,6}[0-9A-Fa-f]{1,4})|(([0-9A-Fa-f]{1,4}:){1,7}:))$/i.test(a)},"Please enter a valid IP v6 address.");
jQuery.validator.addMethod("pattern",function(a,b,c){return this.optional(b)||c.test(a)},"Invalid format.");
;
steal.end();
$.Model.extend("Mgmtransformer.Models.Stock",{id:"stockid",attributes:{price:"money",kitprice:"money",kva:"round",height:"round",width:"round",depth:"round",weight:"round"},convert:{money:function(a){return $.formatNumber(a,{format:"#,###",locale:"us"})},round:function(a){return parseFloat(a)}},models:function(a){return this._super(a.Stock)},findAll:function(a,b,c){$.ajax({url:"Ajax.aspx?action=Stock",type:"get",dataType:"json stock.models",data:$.extend({},a),contentType:"application/json",processdata:true,
success:b,error:c,cache:false})}},{});
;
steal.end();
$.Model.extend("Mgmtransformer.Models.Quote",{id:"quoteid",attributes:{createdate:"date"},convert:{date:function(a){return(new Date(a)).format("yyyy-mm-dd")}},models:function(a){return this._super(a.Quotes)},model:function(a){return a.QuoteDetails?this._super({quoteid:a.quoteID,quotedetails:Mgmtransformer.Models.Quotedetail.models(a.QuoteDetails),customer:Mgmtransformer.Models.Customer.model(a.Customer[0])}):this._super(a)},findAll:function(a,b,c){$.ajax({url:"Ajax.aspx?action=Quotes",type:"get",
dataType:"json quote.models",data:a,success:b,error:c,cache:false})},findOne:function(a,b,c){$.ajax({url:"Ajax.aspx?action=Quote",type:"get",dataType:"json quote.model",data:a,success:b,error:c,cache:false})},copy:function(a,b,c){$.ajax({url:"Ajax.aspx?action=QuoteCopy",type:"post",dataType:"json",data:a,success:function(d){b(d)},error:c,cache:false})},clear:function(a,b,c){$.ajax({url:"Ajax.aspx?action=QuoteDelete",type:"post",dataType:"json",data:a,success:function(d){b(d)},error:c,cache:false})}},
{});
;
steal.end();
$.Model.extend("Mgmtransformer.Models.Quotedetail",{id:"quoteDetailsID",attributes:{price:"money",customStockPrice:"money",totalprice:"money",kitPrice:"money",stockKVA:"round",customStockKVA:"round",customStockTemperature:"round"},convert:{money:function(b){return $.formatNumber(b,{format:"#,###",locale:"us"})},round:function(b){return parseFloat(b)}},create:function(b,a,c){$.ajax({url:"Ajax.aspx?action=QuoteDetailCreate",type:"get",dataType:"json",contentType:"application/json; charset=utf-8",data:$.extend({quoteid:0,
stockid:0,customid:0,kitID:0,primaryVoltage:0,primaryVoltageDW:"",secondaryVoltage:0,secondaryVoltageDW:"",taps:0,customCatalogNumber:"",quantity:0,updateQuantity:false},b),success:a,error:c,cache:false})},update:function(b,a,c,d){$.ajax({url:"Ajax.aspx?action=QuoteDetailCreate",type:"get",dataType:"json",contentType:"application/json; charset=utf-8",data:{quoteid:a.quoteID,stockid:a.stockUnitID,customid:a.customStockID,kitID:a.kitID,primaryVoltage:a.customPrimaryVoltage,primaryVoltageDW:a.customPrimaryVoltageDW,
secondaryVoltage:a.customSecondaryVoltage,secondaryVoltageDW:a.customSecondaryVoltageDW,taps:a.taps,customCatalogNumber:a.customCatalogNumber,quantity:a.quantity,updateQuantity:true},success:c,error:d,cache:false})},destroy:function(b,a,c){return $.ajax({url:"Ajax.aspx?action=QuoteDetailDelete&quoteDetailId="+b+"&quoteID="+this.attributes.quoteID,type:"get",dataType:"json",success:a,error:c,cache:false})}},{formattedprimaryvoltage:function(){return this.formattedvoltage(this.customPrimaryVoltage,
this.customPrimaryVoltageDW)},formattedsecondaryvoltage:function(){return this.formattedvoltage(this.customSecondaryVoltage,this.customSecondaryVoltageDW)},formattedvoltage:function(b,a){return a=="Y"?b+"Y/"+Math.round(parseInt(b)/Math.sqrt(3)):b+"V"}});
;
steal.end();
$.Model.extend("Mgmtransformer.Models.Stockphase",{findAll:function(a,b,c){$.ajax({url:"Ajax.aspx?action=StockPhase",type:"get",dataType:"json stockphase.models",data:$.extend({winding:"Aluminum"},a),contentType:"application/json; charset=utf-8",processdata:true,success:b,error:c,cache:false})}},{});
;
steal.end();
$.Model.extend("Mgmtransformer.Models.Stockconfiguration",{findAll:function(a,b,c){$.ajax({url:"Ajax.aspx?action=StockConfiguration",type:"get",dataType:"json stockconfiguration.models",data:$.extend({winding:"Aluminum",phase:"Three",kva:15},a),contentType:"application/json; charset=utf-8",processdata:true,success:b,error:c,cache:false})}},{});
;
steal.end();
$.Model.extend("Mgmtransformer.Models.Stockkva",{attributes:{kva:"kva"},convert:{kva:function(a){return parseFloat(a)}},findAll:function(a,b,c){$.ajax({url:"Ajax.aspx?action=StockKVA",type:"get",dataType:"json stockkva.models",data:$.extend({winding:"Aluminum",phase:"Three"},a),contentType:"application/json; charset=utf-8",processdata:true,success:b,error:c,cache:false})}},{});
;
steal.end();
$.Model.extend("Mgmtransformer.Models.Stockcatalog",{attributes:{price:"money",kitPrice:"money"},convert:{money:function(a){return $.formatNumber(a,{format:"#,###",locale:"us"})}},findAll:function(a,b,c){$.ajax({url:"Ajax.aspx?action=StockCatalog",type:"get",dataType:"json stockcatalog.models",data:$.extend({winding:"Aluminum",phase:"Three",config:"",kva:""},a),contentType:"application/json; charset=utf-8",processdata:true,success:b,error:c,cache:false})}},{});
;
steal.end();
$.Model.extend("Mgmtransformer.Models.Customcatalog",{attributes:{price:"money",kitPrice:"money"},convert:{money:function(a){return $.formatNumber(a,{format:"#,###",locale:"us"})},round:function(a){return parseFloat(a)}},findOne:function(a,b,c){$.ajax({url:"Ajax.aspx?action=GetCustomCatalogNumber",type:"get",dataType:"json customcatalog.model",data:$.extend({winding:"Aluminum",phase:"Three"},a),contentType:"application/json; charset=utf-8",processdata:true,success:b,error:c,cache:false})}},{});
;
steal.end();
$.Model.extend("Mgmtransformer.Models.Customer",{id:"customerID",findOne:function(a,b,c){$.ajax({url:"/RepStockUnits.svc/json/customers",type:"get",dataType:"json customer.model",data:$.extend({winding:"Aluminum",phase:"Three"},a),contentType:"application/json; charset=utf-8",processdata:true,success:b,error:c,cache:false,fixture:"//mgmtransformer/fixtures/customer.json.get"})},finalize:function(a,b,c){$.ajax({url:"Ajax.aspx?action=FinalizeQuote",type:"get",dataType:"json",contentType:"application/json; charset=utf-8",
success:b,error:c,cache:false,data:a})}},{});
;
steal.end();
$.Controller.extend("Mgmtransformer.Controllers.Stock",{},{init:function(){this.element.addClass("mgmtransformer_stock");this.element.html(this.view("init",{configurations:this.options.configurations}));Mgmtransformer.Models.Stock.findAll({winding:this.options.configurations[0].Windings,phase:this.options.configurations[0].Phase,config:this.options.configurations[0].Configuration},this.callback("list"))},showloader:function(){this.element.find(".loader").length==0&&this.element.append($.View("//mgmtransformer/views/loader/loader.ejs"))},
hideloader:function(){this.element.find(".loader").remove()},list:function(a){this.element.find("div#stock-table").html(this.view("list",{stocks:a}));this.hideloader()},"select change":function(a){a=$(a).val();if(a!=""){this.showloader();a=a.split("||");Mgmtransformer.Models.Stock.findAll({winding:a[0],phase:a[1],config:a[2]},this.callback("list"))}}});
;
steal.end();
$.Controller.extend("Mgmtransformer.Controllers.Quote",{},{init:function(){this.element.addClass("mgmtransformer_quote").find(".quotecustomer").mgmtransformer_customer().end().find(".quotedetaillist").mgmtransformer_quotedetail().end();Mgmtransformer.Models.Quote.findAll({},this.callback("list"))},showloader:function(){this.element.find(".loader").length==0&&this.element.append($.View("//mgmtransformer/views/loader/loader.ejs"))},hideloader:function(){this.element.find(".loader").remove()},list:function(a){this.element.html(this.view("init",
{quotes:a,isHistory:this.options.isHistory}));this.element.find(".quotecustomer").mgmtransformer_customer();this.element.find(".quotedetaillist").mgmtransformer_quotedetail();this.hideloader()},displaylist:function(){this.element.find(".quoteedit, .quoteshow").hide().end().find(".quotelist").show()},showfirstquote:function(){this.element.find(".quotelist, .quoteedit, .quoteshow").hide().end()},".quotelist .show click":function(a){a=a.closest(".quote").model().quoteid;if(a!=0){this.showloader();Mgmtransformer.Models.Quote.findOne({quoteid:a},
this.callback("show"))}},show:function(a){this.element.find(".quotelist, .quoteedit").hide().end().find(".quoteshow").find("h2 span").html(a.quoteid).end().find(".quotedetaillist").mgmtransformer_quotedetail("list",a.quotedetails,true).end().find(".quotecustomer").mgmtransformer_customer("show",a.customer).end().show().end();this.hideloader()},"#btn-history click":function(){this.element.find(".quoteshow").hide().end().find(".quotelist").show().end()},"#btn-copyquote click":function(){this.element.trigger("copyquote",
parseInt(this.element.find(".quoteshow h2 span").text()))},showcart:function(a){Mgmtransformer.Models.Quote.findOne({quoteid:a},this.callback("edit"))},edit:function(a){this.element.show().find(".quotelist, .quoteshow, .quoteedit #btn-finalquote, .quoteedit .quotecustomer, .quoteedit  #btn-summary").hide().end().find(".quoteedit").find("h2 span").html(a.quoteid).end().find(".quotedetaillist").mgmtransformer_quotedetail("list",a.quotedetails,false).end().find(".quotecustomer").mgmtransformer_customer("edit",
{customer:a.customer,quoteid:a.quoteid}).unbind("loader").bind("loader",this.callback("showloader")).unbind("hideloader").bind("hideloader",this.callback("hideloader")).unbind("finalized").bind("finalized",this.callback("finalized")).end().find("#btn-customer, .quotedetaillist").show().end().show().end()},"#btn-addquote click":function(){this.element.trigger("return")},"#btn-customer click":function(){this.element.find(".quoteedit").find(".quotedetaillist, #btn-customer").hide().end().find(".quotecustomer, #btn-finalquote, #btn-summary").show().end()},
"#btn-summary click":function(){this.element.find(".quoteedit").find(".quotecustomer, #btn-finalquote, #btn-summary").hide().end().find(".quotedetaillist, #btn-customer").show().end()},"#btn-finalquote click":function(){this.element.find(".quoteedit .quotecustomer").mgmtransformer_customer("finalize",parseInt(this.element.find(".quoteedit h2 span").html()))},emptyCart:function(){this.element.find(".quotedetaillist").mgmtransformer_quotedetail("empty").end()},finalized:function(){var a=$("#quoteID").val();
Mgmtransformer.Models.Quote.findAll({},this.callback("list"));Mgmtransformer.Models.Quote.findOne({quoteid:a},this.callback("show"));this.showfirstquote();this.element.find(".quoteedit .quotedetaillist").mgmtransformer_quotedetail("resetquotedetail").end().trigger("finalized")}});
;
steal.end();
$.Controller.extend("Mgmtransformer.Controllers.Newquote",{},{init:function(){var a=this;$.each(this.options.CustomKvaJson,function(b,c){a.options.CustomKvaJson[b].KVA=parseFloat(c.KVA)});$.each(this.options.CustomTempJson,function(b,c){a.options.CustomTempJson[b].Temperature=parseFloat(c.Temperature)});this.element.addClass("mgmtransformer_newquote").html(this.view("init",this.options));this.$stockunit=this.element.find("#stock-unit");this.$specialunit=this.element.find("#special-unit");this.element.find("#quoteID").val(this.options.Quoteid).end()},
reset:function(){this.resetcustomseletion();this.resetstockselection()},showloader:function(a){a.find(".loader").length==0&&a.append($.View("//mgmtransformer/views/loader/loader.ejs"))},hideloader:function(a){a.find(".loader").remove()},confirmation:function(){$(".cart").show();$("#btn-gotoquote").click()},".required focus":function(a){$(a).removeClass("error")},"input.quantity blur":function(a){$(a).val()==""&&$(a).val(0)},"input.quantity keypress":function(a,b){return b.which!=8&&b.which!=0&&(b.which<
48||b.which>57)?false:true},"input.quantity-btn click":function(a){var b=$(a).siblings(".quantity");if(!b.hasClass("disabled"))if($(a).hasClass("add"))b.val(parseInt(b.val())+1).trigger("change");else parseInt(b.val())<=0?b.val(0).trigger("change"):b.val(parseInt(b.val())-1).trigger("change")},valid:function(a,b){var c=true,d=a+" .required";if(b)d=a+" .required:not(.quantity)";$(d).each(function(e,f){if($(f).val()==""||$(f).val()<=0){b||$(f).addClass("error");c=false}});return c},clearerror:function(){this.element.find(".error").removeClass("error")},
copyquote:function(a){if(this.element.find("#quoteID").val()!=0){alert("You must finalize the cart before you can copy a quote.");return false}else Mgmtransformer.Models.Quote.copy({quoteid:a},this.callback("copied"))},copied:function(a){this.element.find("#quoteID").val(a.quoteid).end().trigger("copied");$(".cart").show()},finalized:function(){this.element.find("#quoteID").val(0)},"li#stock-windings input change":function(a){this.showloader($("#newquote"));Mgmtransformer.Models.Stockphase.findAll({winding:$(a).val()},
this.callback("showphase"))},showphase:function(a){this.$stockunit.find("#stock-phase").html(this.view("phase",{phases:a})).end().find("li#stock-phase input:checked").trigger("change")},"li#stock-phase input change":function(a){this.showloader($("#newquote"));Mgmtransformer.Models.Stockkva.findAll({winding:this.$stockunit.find("li#stock-windings input:checked").val(),phase:$(a).val()},this.callback("showkvas"))},showkvas:function(a){this.$stockunit.find("#stock-kva").html(this.view("kva",{kvas:a}));
this.hideloader($("#newquote"));this.$stockunit.find("#stock-configuration").html("").end().find("#stock-catalogNumber").html("<span></span>").end().find("#stock-price").html("<span></span>");this.resetstockcatalog()},"#stock-kva change":function(a){this.showloader($("#newquote"));Mgmtransformer.Models.Stockconfiguration.findAll({winding:this.$stockunit.find("li#stock-windings input:checked").val(),phase:this.$stockunit.find("li#stock-phase input:checked").val(),kva:$(a).val()},this.callback("showconfigs"))},
showconfigs:function(a){this.$stockunit.find("#stock-configuration").html(this.view("configs",{configurations:a}));this.resetstockcatalog();this.hideloader($("#newquote"))},"#stock-configuration change":function(a){if($(a).val()==-1){this.resetstockcatalog();return false}this.showloader($("#newquote"));Mgmtransformer.Models.Stockcatalog.findAll({winding:this.$stockunit.find("li#stock-windings input:checked").val(),phase:this.$stockunit.find("li#stock-phase input:checked").val(),kva:$("#stock-kva").val(),
config:$(a).val()},this.callback("showcatalog"))},showcatalog:function(a){if(a==null){this.resetstockcatalog();return false}a=a[0];this.$stockunit.find("#stock-catalogNumber").html("<span>"+a.catalogNumber+"</span>").end().find("#stock-price").html("<span> $"+a.price+"</span>").end().find("#stockID").val(a.stockID).end();if(a.kit!=""){this.$stockunit.find("#stockkitID").val(a.kitID).end().find("#stock-kit-price").html("<span> $"+a.kitPrice+"</span>").end().find("#stocknone").hide().end().find("#stockkit-quantity").removeAttr("disabled").removeClass("disabled").end();
a.kit=="WB 1"||a.kit=="WB 2"?this.$stockunit.find("#stockrain").hide().end().find("#stockwall").show():this.$stockunit.find("#stockwall").hide().end().find("#stockrain").show()}else this.resetstockkit();this.hideloader($("#newquote"))},resetstockcatalog:function(){this.$stockunit.find("#stock-catalogNumber").html("<span></span>").end().find("#stock-price").html("<span></span>").end();this.resetstockkit()},resetstockkit:function(){this.$stockunit.find("#stockwall").hide().end().find("#stockrain").hide().end().find("#stocknone").hide().end().find("#stockkitID").val(0).end().find("#stockkit-quantity").val(0).attr("disabled",
"disabled").addClass("disabled").end().find("#stock-kit-price").html("<span></span>");var a=this.$stockunit.find("#stock-configuration").val();a!=null&&a!=-1&&this.$stockunit.find("#stocknone").show().end()},"#stock-add click":function(){var a=this.$stockunit.find("#stock-quantity").val(),b=this.$stockunit.find("#stockkit-quantity").val();if(!this.valid("#stock-unit"))return false;this.showloader($("#newquote"));var c=this.callback("updatestockquoteid");if(b>0)c=this.callback("addstockaccessory");
(new Mgmtransformer.Models.Quotedetail({quoteid:$("#quoteID").val(),stockid:this.$stockunit.find("#stockID").val(),quantity:a,kitID:this.$specialunit.find("#specialkitID").val()})).save(c)},addstockaccessory:function(a){$("#quoteID").val(a.quoteid);this.showloader($("#newquote"));(new Mgmtransformer.Models.Quotedetail({quoteid:$("#quoteID").val(),kitID:this.$stockunit.find("#stockkitID").val(),quantity:this.$stockunit.find("#stockkit-quantity").val()})).save(this.callback("updatestockquoteid"))},
updatestockquoteid:function(a){$("#quoteID").val(a.quoteid);this.resetstockselection();this.clearerror();this.confirmation(this.$stockunit.find("#stock-add"))},resetstockselection:function(){this.$stockunit.find("#stock-quantity").val("0").end().find("#stockkit-quantity").val(0);if(this.$stockunit.find("#stock-windings-copper").attr("checked")=="checked"){this.$stockunit.find("#stock-windings-aluminum").click().trigger("change");return true}if(this.$stockunit.find("#stock-phase-single").attr("checked")==
"checked"){this.$stockunit.find("#stock-phase-three").click();return true}this.$stockunit.find("#stock-phase-three").trigger("change")},"#special-unit .required change":function(a){if(!$(a).hasClass("quantity")){this.$specialunit.find("#special-kit-quantity").val(0);this.getcustomcatalog()}},"#special-kit-quantity change":function(){this.getcustomcatalog()},"#special-unit input[type=radio] change":function(){this.$specialunit.find("#special-kit-quantity").val(0);this.getcustomcatalog()},"input.voltage-type change":function(a){var b=
$(a).siblings("select");$(a).val()=="Y"?$.each(b.find("option"),function(c,d){c=$(d).text();var e="";if(c=="")return true;e=$.trim(c)+"Y/"+Math.round(parseInt(c)/Math.sqrt(3));$(d).text(e)}):$.each(b.find("option"),function(c,d){c=$(d).text().split("/");var e=c[0].replace("Y","");if(c=="")return true;$(d).text(e)})},getcustomcatalog:function(){if(this.valid("#special-unit",true))Mgmtransformer.Models.Customcatalog.findOne({winding:this.$specialunit.find("li#special-windings input:checked").val(),
phase:this.$specialunit.find("li#special-phase input:checked").val(),kva:this.$specialunit.find("#special-kva").val(),temp:this.$specialunit.find("#special-temp").val(),kfactor:this.$specialunit.find("#special-kfactor").val(),sound:this.$specialunit.find("#special-sound").val(),pvolt:this.$specialunit.find("#special-primary-voltage").val(),svolt:this.$specialunit.find("#special-secondary-voltage").val(),pvoltdw:this.$specialunit.find('input[name="special-primary-dw"]:checked').val(),svoltdw:this.$specialunit.find('input[name="special-secondary-dw"]:checked').val(),
taps:this.$specialunit.find("#special-taps").val(),kit:this.$specialunit.find("#special-kit-quantity").val()>0?this.$specialunit.find("#specialkitID").val():0},this.callback("showcustomcatalog"));else{this.resetcustomcatalog();return false}},showcustomcatalog:function(a){this.$specialunit.find("#special-catalogNumber").html("<span>"+a.catalogNumber+"</span>").end().find("#specialID").val(a.customID).end();var b='<span class="consult">Consult factory for pricing</span>';if(a.price!="0"){b="$"+a.price;
this.$specialunit.find("#special-quantity").val(0).removeAttr("disabled").removeClass("disabled").end()}else this.$specialunit.find("#special-quantity").val(0).attr("disabled","disabled").addClass("disabled").end();this.$specialunit.find("#special-price").html("<span>"+b+"</span>").end();if(a.kit!=""){this.$specialunit.find("#specialkitID").val(a.kitID).end().find("#special-kit-price").html("<span> $"+a.kitPrice+"</span>").end().find("#special-kit-quantity").removeAttr("disabled").removeClass("disabled").end().find("#specialnone").hide().end().find("#specialwall").hide().end().find("#specialrain").hide().end();
if(a.kit=="WB 1"||a.kit=="WB 2")this.$specialunit.find("#specialwall").show();else if(a.kit=="RH 5"||a.kit=="RH 6")this.$specialunit.find("#specialrain").show();this.$specialunit.find("#specialkitID").val(a.kitID).end().find("#specialkkit").css({visibility:"visible"})}else this.resetcustomkit(a.price!="0")},resetcustomseletion:function(){this.$specialunit.find("#special-quantity").val("0").end().find("#special-kva").val("-1").end().find("#special-temp").val("-1").end().find("#special-kfactor").val("-1").end().find("#special-sound").val("-1").end().find("#special-primary-voltage").val("-1").end().find("#special-primary-delta").click().end().find("#special-secondary-voltage").val("-1").end().find("#special-taps").val("-1").end().find("#special-windings-aluminum").click().end().find("#special-secondary-delta").click().end();
this.resetcustomcatalog()},resetcustomcatalog:function(){this.$specialunit.find("#special-catalogNumber").html("<span></span>").end().find("#special-price").html("<span></span>").end();this.resetcustomkit()},resetcustomkit:function(a){this.$specialunit.find("#specialwall").hide().end().find("#specialrain").hide().end().find("#specialkitID").val(0).end().find("#special-kit-quantity").val(0).attr("disabled","disabled").addClass("disabled").end().find("#special-kit-price").html("<span></span>");a?this.$specialunit.find("#specialnone").find("label").html(this.$specialunit.find("#special-kva").val()<
500?"This item is already rated Indoor/Outdoor or not eligible for a wall bracket.":"This item is rated indoor only, for outdoor applications consult factory. <br/>This unit is not eligible for a wall bracket.").end().show().end():this.$specialunit.find("#specialnone").hide().end()},"#special-add click":function(){this.$specialunit.find("#special-quantity").val();var a=this.$specialunit.find("#special-kit-quantity").val();if(!this.valid("#special-unit"))return false;var b=this.callback("updatecustomquoteid");
if(a>0)b=this.callback("addspecialaccessory");this.showloader($("#newquote"));(new Mgmtransformer.Models.Quotedetail({quoteid:$("#quoteID").val(),customid:this.$specialunit.find("#specialID").val(),quantity:this.$specialunit.find("#special-quantity").val(),primaryVoltage:this.$specialunit.find("#special-primary-voltage").val(),primaryVoltageDW:this.$specialunit.find('input[name="special-primary-dw"]:checked').val(),secondaryVoltage:this.$specialunit.find("#special-secondary-voltage").val(),secondaryVoltageDW:this.$specialunit.find('input[name="special-secondary-dw"]:checked').val(),
taps:this.$specialunit.find("#special-taps").val(),customCatalogNumber:this.$specialunit.find("#special-catalogNumber").text(),kitID:this.$specialunit.find("#special-kit-quantity").val()>0?this.$specialunit.find("#specialkitID").val():0})).save(b)},addspecialaccessory:function(a){$("#quoteID").val(a.quoteid);(new Mgmtransformer.Models.Quotedetail({quoteid:$("#quoteID").val(),kitID:this.$specialunit.find("#specialkitID").val(),quantity:this.$specialunit.find("#special-kit-quantity").val()})).save(this.callback("updatecustomquoteid"))},
updatecustomquoteid:function(a){$("#quoteID").val(a.quoteid);this.resetcustomseletion();this.clearerror();this.confirmation(this.$specialunit.find("#special-add"));this.hideloader($("#newquote"))},"#btn-gotoquote click":function(){$("#quoteID").val()!=0?this.element.trigger("showdetail",[$("#quoteID").val(),false]):alert("The cart is empty")}});
;
steal.end();
$.Controller.extend("Mgmtransformer.Controllers.Quotedetail",{},{init:function(){this.element.addClass("mgmtransformer_quotedetail");this.element.html(this.view("empty"));this.isHistory=false},resetquotedetail:function(){this.element.html(this.view("empty"))},list:function(a,b){var c=false;$.each(a,function(e,d){if(parseInt(d.kitID)!=0)c=true});if(a.length>0){this.element.html(this.view("init",{quotedetails:a,isHistory:b,haskit:c}));this.updateprice()}else b||this.empty()},empty:function(){this.element.html(this.view("empty"))},
".qtyupdate click":function(a){var b=$(a).closest("tr").model();a=$(a).closest("tr").find("input.quantity").val();if(a==b.quantity)return false;else b.quantity=a;this.showloader();b.update({updateQuantity:true,quantity:b.quantity},this.callback("updateprice"))},".remove click":function(a){$(a).closest("tr").model().destroy().done(function(b,c){$(a).closest("tr").remove();if(c.quoteID==0){$(".cart").hide();$("#quoteID").val(0)}},this.callback("updateprice"))},removerow:function(){this.updateprice()},
updateprice:function(){var a=0;if(this.element.find("tr.quotedetail").length>0){this.element.find("tr.quotedetail").each(function(){var b=$(this).model();b=parseInt(b.price.replace(",",""))*b.quantity;a+=b;$(this).find(".totalprice").text("$"+$.formatNumber(b,{format:"#,###",locale:"us"}))});this.element.find("#totalprice").text($.formatNumber(a,{format:"#,###",locale:"us"}))}else this.empty();this.hideloader()},showloader:function(){this.element.find(".loader").length==0&&this.element.append($.View("//mgmtransformer/views/loader/loader.ejs"))},
hideloader:function(){this.element.find(".loader").remove()},"input.quantity blur":function(a){$(a).val()==""&&$(a).val(0)},"input.quantity keypress":function(a,b){return b.which!=8&&b.which!=0&&(b.which<48||b.which>57)?false:true}});
;
steal.end();
$.Controller.extend("Mgmtransformer.Controllers.Customer",{},{init:function(){this.element.addClass("mgmtransformer_customer");this.element.html(this.view("init"))},"#issamebilling change":function(){this.element.find("#issamebilling:checked").length>0?this.element.find("#shipping input[type=text], #shipping select").attr("disabled",true).end().find("#shipcompany").val(this.element.find("#company").val()).end().find("#shipaddressline1").val(this.element.find("#addressline1").val()).end().find("#shipaddressline2").val(this.element.find("#addressline2").val()).end().find("#shipcity").val(this.element.find("#city").val()).end().find("#shipstate").val(this.element.find("#state").val()).end().find("#shipzip").val(this.element.find("#zip").val()).end():
this.element.find("#shipping input[type=text], #shipping select").removeAttr("disabled")},show:function(a){this.element.html(this.view("show",{customer:a}))},edit:function(a){var e=a.quoteid;a=a.customer;jQuery.validator.messages.required="";this.element.html(this.view("edit",{customer:a,quoteid:e}));this.element.find("#state").val(a.state).end().find("#shipstate").val(a.shipstate).end().find("input.phone").mask("(999) 999-9999").end().find("input.zip").mask("99999").end().find("#customerform").validate({invalidHandler:function(b,
c){if(b=c.numberOfInvalids()){b=b==1?"You missed 1 field. It has been highlighted above":"You missed "+b+" fields. They have been highlighted.";$("#customerErrorBox span").html(b);$("#customerErrorBox").show()}else $("#customerErrorBox").hide()},submitHandler:function(){$("#customerErrorBox").hide();alert("submit! use link below to go to the other step")},focusInvalid:false,focusCleanup:true,onkeyup:false,errorElement:"em",wrapper:"li",errorLabelContainer:"#customerErrorBox ul",showErrors:function(b,
c){$("#customerErrorBox ul").html("");c.length>0&&$.each(c,function(f,d){$(d.element).addClass("error");d.message!=""&&$("#customerErrorBox ul").append("<li>"+d.message+"</li>")})}});a.issamebilling!="True"&&this.element.find("#shipping input[type=text], #shipping select").removeAttr("disabled")},finalize:function(){if(this.element.find("#customerform").validate().form()){this.element.trigger("loader");Mgmtransformer.Models.Customer.finalize($.extend(this.element.find("form").formParams(),{issamebilling:this.element.find("#issamebilling:checked").length>
0}),this.callback("finalizecleanup"))}},finalizecleanup:function(a){if(a.error=="")this.element.html(this.view("init")).trigger("finalized");else{$("#customerErrorBox ul").html("").append("<li>"+a.error+"</li>");this.element.trigger("hideloader")}}});
;
steal.end();
$.View.preload('mgmtransformer_views_newquote_configs_ejs',jQuery.EJS(function(_CONTEXT,_VIEW) { try { with(_VIEW) { with (_CONTEXT) {var ___v1ew = [];___v1ew.push("﻿<option value=\"-1\"></option>\n");
for(var i = 0; i < configurations.length ; i++){;___v1ew.push("\n");
___v1ew.push("<option value=\"");___v1ew.push((jQuery.EJS.text(configurations[i].configuration)));___v1ew.push("\">");___v1ew.push((jQuery.EJS.text( configurations[i].configuration)));___v1ew.push("\n");
___v1ew.push("</option>\n");
};; return ___v1ew.join('');}}}catch(e){e.lineNumber=null;throw e;} }));;
steal.end();
$.View.preload('mgmtransformer_views_newquote_init_ejs',jQuery.EJS(function(_CONTEXT,_VIEW) { try { with(_VIEW) { with (_CONTEXT) {var ___v1ew = [];___v1ew.push("﻿<h2>\n");
___v1ew.push("  Enter New Quote\n");
___v1ew.push("</h2>\n");
___v1ew.push("<div id=\"stock-unit\">\n");
___v1ew.push("  <input id=\"quoteID\" type=\"hidden\" value=\"0\"/>\n");
___v1ew.push("  <h3>\n");
___v1ew.push("    Standard/Stock Unit\n");
___v1ew.push("  </h3>\n");
___v1ew.push("  <ul>\n");
___v1ew.push("    <li id=\"stock-windings\">\n");
___v1ew.push("      <label class=\"master\">Windings</label>\n");
___v1ew.push("      <input name=\"stock-windings\" id=\"stock-windings-aluminum\" type=\"radio\" value=\"Aluminum\" checked=\"checked\"/>\n");
___v1ew.push("      <label for=\"stock-windings-aluminum\">\n");
___v1ew.push("        Aluminum\n");
___v1ew.push("      </label>\n");
___v1ew.push("      <input name=\"stock-windings\" id=\"stock-windings-copper\" type=\"radio\" value=\"Copper\" />\n");
___v1ew.push("      <label for=\"stock-windings-copper\">\n");
___v1ew.push("        Copper\n");
___v1ew.push("      </label>\n");
___v1ew.push("    </li>\n");
___v1ew.push("    <li id=\"stock-phase\">\n");
___v1ew.push("      <label class=\"master\">Phase</label>\n");
___v1ew.push("      <input name=\"stock-phase\" id=\"stock-phase-single\" type=\"radio\" value=\"single\" />\n");
___v1ew.push("      <label for=\"stock-phase-single\">\n");
___v1ew.push("        Single Phase\n");
___v1ew.push("      </label>\n");
___v1ew.push("      <input name=\"stock-phase\" id=\"stock-phase-three\" type=\"radio\" value=\"three\" checked=\"checked\"/>\n");
___v1ew.push("      <label for=\"stock-phase-three\">\n");
___v1ew.push("        Three Phase\n");
___v1ew.push("      </label>\n");
___v1ew.push("    </li>\n");
___v1ew.push("    <li class=\"id\">\n");
___v1ew.push("      <label for=\"stock-catalogNumber\" class=\"master\">\n");
___v1ew.push("        Catalog No.\n");
___v1ew.push("      </label>\n");
___v1ew.push("      <span id=\"stock-catalogNumber\"></span>\n");
___v1ew.push("    </li>\n");
___v1ew.push("    <li class=\"id\">\n");
___v1ew.push("      <label for=\"stock-quantity\" class=\"master\">\n");
___v1ew.push("        Qty\n");
___v1ew.push("      </label>\n");
___v1ew.push("      <input type=\"button\" class=\"subtract quantity-btn\" value=\"-\">/<input type=\"button\" class=\"add quantity-btn\" value=\"+\"><input type=\"text\" id=\"stock-quantity\" value=\"0\" class='quantity required'/>\n");
___v1ew.push("          \n");
___v1ew.push("    </li>\n");
___v1ew.push("    <li>\n");
___v1ew.push("      <label for=\"stock-kva\" class=\"master\">\n");
___v1ew.push("        KVA\n");
___v1ew.push("      </label>\n");
___v1ew.push("      <select id=\"stock-kva\" class=\"required\"></select>\n");
___v1ew.push("    </li>\n");
___v1ew.push("    <li>\n");
___v1ew.push("      <label for=\"stock-configuration\" class=\"master\">\n");
___v1ew.push("        Configuration\n");
___v1ew.push("      </label>\n");
___v1ew.push("      <select id=\"stock-configuration\" class=\"required\"></select>\n");
___v1ew.push("    </li>\n");
___v1ew.push("    <li class=\"id\">\n");
___v1ew.push("      <label for=\"stock-price\" class=\"master\">\n");
___v1ew.push("        Price\n");
___v1ew.push("      </label>\n");
___v1ew.push("      <span id=\"stock-price\"></span>\n");
___v1ew.push("    </li>\n");
___v1ew.push("  </ul>\n");
___v1ew.push("  \n");
___v1ew.push("  \n");
___v1ew.push("  <ul id=\"stockkit\"  class='kit'>\n");
___v1ew.push("    <li class=\"double clear\">\n");
___v1ew.push("      <h3>Accesories</h3>\n");
___v1ew.push("      <div id=\"stockwall\" class=\"accessories\">\n");
___v1ew.push("        <label for=\"accessories-wall\">\n");
___v1ew.push("          Wall Bracket\n");
___v1ew.push("        </label>\n");
___v1ew.push("      </div>\n");
___v1ew.push("      <div id=\"stockrain\" class=\"accessories\">\n");
___v1ew.push("        <label for=\"stock-accessories-rain\">\n");
___v1ew.push("          Rain Hood\n");
___v1ew.push("        </label>\n");
___v1ew.push("      </div>\n");
___v1ew.push("        <div id=\"stocknone\" class=\"accessories\">\n");
___v1ew.push("            <label for=\"stock-accessories-none\">\n");
___v1ew.push("                This item is already rated Indoor/Outdoor or not eligible for a wall bracket.  \n");
___v1ew.push("            </label>\n");
___v1ew.push("        </div>\n");
___v1ew.push("    </li>\n");
___v1ew.push("    <li class=\"id\">\n");
___v1ew.push("      <label for=\"stock-kit-price\" class=\"master\">\n");
___v1ew.push("        Kit Price\n");
___v1ew.push("      </label>\n");
___v1ew.push("      <span id=\"stock-kit-price\"></span>\n");
___v1ew.push("    </li>\n");
___v1ew.push("    <li class=\"id\">\n");
___v1ew.push("      <label for=\"stockkit-quantity\" class=\"master\">Qty</label>\n");
___v1ew.push("      <input type='hidden' id='stockkitID'/>\n");
___v1ew.push("      <input type=\"button\" class=\"subtract quantity-btn\" value=\"-\">/<input type=\"button\" class=\"add quantity-btn\" value=\"+\"><input type=\"text\" id=\"stockkit-quantity\" value=\"0\"  class='quantity'/>\n");
___v1ew.push("    </li>\n");
___v1ew.push("  </ul>\n");
___v1ew.push("  \n");
___v1ew.push("  <input type='hidden' id='stockID' value=\"0\"/>\n");
___v1ew.push("  <input id=\"stock-add\" type=\"button\" value=\"Add to Quote\" class=\"action\"/>\n");
___v1ew.push("</div>\n");
___v1ew.push("\n");
___v1ew.push("<div id=\"special-unit\">\n");
___v1ew.push("  <h3>\n");
___v1ew.push("    Special Configuration\n");
___v1ew.push("  </h3>\n");
___v1ew.push("  <ul>\n");
___v1ew.push("    <li id=\"special-windings\">\n");
___v1ew.push("      <label class=\"master\">Windings</label>\n");
___v1ew.push("      <input name=\"special-windings\" id=\"special-windings-aluminum\" type=\"radio\" value=\"aluminum\" checked=\"checked\" />\n");
___v1ew.push("      <label for=\"special-windings-aluminum\">\n");
___v1ew.push("        Aluminum\n");
___v1ew.push("      </label>\n");
___v1ew.push("      <input name=\"special-windings\" id=\"special-windings-copper\" type=\"radio\" value=\"copper\" />\n");
___v1ew.push("      <label for=\"special-windings-copper\">\n");
___v1ew.push("        Copper\n");
___v1ew.push("      </label>\n");
___v1ew.push("    </li>\n");
___v1ew.push("    <li id=\"special-phase\">\n");
___v1ew.push("      <label class=\"master\">Phase</label>\n");
___v1ew.push("      <input name=\"special-phase\" id=\"special-phase-three\" type=\"radio\" value=\"three\"  checked=\"checked\"/>\n");
___v1ew.push("      <label for=\"special-phase-three\">\n");
___v1ew.push("        Three Phase\n");
___v1ew.push("      </label>\n");
___v1ew.push("    </li>\n");
___v1ew.push("    <li class=\"id\">\n");
___v1ew.push("      <label for=\"special-catalogNumber\" class=\"master\">\n");
___v1ew.push("        Catalog No.\n");
___v1ew.push("      </label>\n");
___v1ew.push("      <span id=\"special-catalogNumber\"></span>\n");
___v1ew.push("    </li>\n");
___v1ew.push("    <li class=\"id\">\n");
___v1ew.push("      <label for=\"special-quantity\" class=\"master\">\n");
___v1ew.push("        Qty\n");
___v1ew.push("      </label>\n");
___v1ew.push("      <input type=\"button\" class=\"subtract quantity-btn\" value=\"-\">/<input type=\"button\" class=\"add quantity-btn\" value=\"+\"><input type=\"text\" id=\"special-quantity\" value=\"0\"  class='quantity required'/>\n");
___v1ew.push("    </li>\n");
___v1ew.push("    <li>\n");
___v1ew.push("      <label for=\"special-kva\" class=\"master\">\n");
___v1ew.push("        KVA\n");
___v1ew.push("      </label>\n");
___v1ew.push("      <select id=\"special-kva\" class=\"required\">\n");
___v1ew.push("        <option value=\"-1\"></option>\n");
___v1ew.push("        ");for(var i =0; i< CustomKvaJson.length ; i++){;___v1ew.push("\n");
___v1ew.push("        <option value=\"");___v1ew.push((jQuery.EJS.text( CustomKvaJson[i].KVA)));___v1ew.push("\">");___v1ew.push((jQuery.EJS.text( CustomKvaJson[i].KVA)));___v1ew.push("\n");
___v1ew.push("        </option>\n");
___v1ew.push("        ");};___v1ew.push("</select>\n");
___v1ew.push("    </li>\n");
___v1ew.push("    <li>\n");
___v1ew.push("      <label for=\"special-temp\" class=\"master\">\n");
___v1ew.push("        Temp Rise\n");
___v1ew.push("      </label>\n");
___v1ew.push("      <select id=\"special-temp\" class=\"required\">\n");
___v1ew.push("        <option value=\"-1\"></option>\n");
___v1ew.push("        ");for(var i = 0; i < CustomTempJson.length ; i++){;___v1ew.push("\n");
___v1ew.push("        <option value=\"");___v1ew.push((jQuery.EJS.text( CustomTempJson[i].Temperature)));___v1ew.push("\">");___v1ew.push((jQuery.EJS.text( CustomTempJson[i].Temperature)));___v1ew.push("\n");
___v1ew.push("        </option>\n");
___v1ew.push("        ");};___v1ew.push("\n");
___v1ew.push("     </select>\n");
___v1ew.push("    </li>\n");
___v1ew.push("    <li class=\"id\">\n");
___v1ew.push("      <label for=\"special-price\" class=\"master\">\n");
___v1ew.push("        Price\n");
___v1ew.push("      </label>\n");
___v1ew.push("      <span id=\"special-price\"></span>\n");
___v1ew.push("    </li>\n");
___v1ew.push("    <li class='clear'>\n");
___v1ew.push("      <label for=\"special-kfactor\" class=\"master\">\n");
___v1ew.push("        K-Factor\n");
___v1ew.push("      </label>\n");
___v1ew.push("      <select id=\"special-kfactor\" class=\"required\">\n");
___v1ew.push("        <option value=\"-1\"></option>\n");
___v1ew.push("        ");for(var i = 0; i < CustomKFactorJson.length ; i++){;___v1ew.push("\n");
___v1ew.push("        <option value=\"");___v1ew.push((jQuery.EJS.text( CustomKFactorJson[i].KFactor)));___v1ew.push("\">\n");
___v1ew.push("            ");___v1ew.push((jQuery.EJS.text( CustomKFactorJson[i].KFactor)));___v1ew.push(" \n");
___v1ew.push("            ");if (CustomKFactorJson[i].KFactor == "K-1"){;___v1ew.push("\n");
___v1ew.push("                (Standard)\n");
___v1ew.push("            ");};___v1ew.push("\n");
___v1ew.push("        </option>\n");
___v1ew.push("        ");};___v1ew.push("</select>\n");
___v1ew.push("    </li>\n");
___v1ew.push("\n");
___v1ew.push("    <li>\n");
___v1ew.push("      <label for=\"special-sound\" class=\"master\">\n");
___v1ew.push("        Sound Level\n");
___v1ew.push("      </label>\n");
___v1ew.push("      <select id=\"special-sound\" class=\"required\">\n");
___v1ew.push("        <option value=\"-1\"></option>\n");
___v1ew.push("        ");for(var i = 0; i < SoundLevel.length ; i++){;___v1ew.push("\n");
___v1ew.push("            <option value=\"");___v1ew.push((jQuery.EJS.text( SoundLevel[i].SoundLevel)));___v1ew.push("\">\n");
___v1ew.push("                ");___v1ew.push((jQuery.EJS.text( SoundLevel[i].Description)));___v1ew.push("\n");
___v1ew.push("                ");if (SoundLevel[i].Description == "NEMA ST-20"){;___v1ew.push("\n");
___v1ew.push("                    (Standard)\n");
___v1ew.push("                ");};___v1ew.push("\n");
___v1ew.push("            </option>\n");
___v1ew.push("        ");};___v1ew.push("\n");
___v1ew.push("      </select>\n");
___v1ew.push("    </li>\n");
___v1ew.push("    <li class='clear'>\n");
___v1ew.push("      <label for=\"special-primary-voltage\" class=\"master\">\n");
___v1ew.push("        Primary Voltage\n");
___v1ew.push("      </label>\n");
___v1ew.push("      <select id=\"special-primary-voltage\" class=\"required\">\n");
___v1ew.push("        <option value=\"-1\"></option>\n");
___v1ew.push("        ");for(var i = 0; i < Voltage.length ; i++){;___v1ew.push("\n");
___v1ew.push("        <option value=\"");___v1ew.push((jQuery.EJS.text( Voltage[i].Voltage)));___v1ew.push("\">");___v1ew.push((jQuery.EJS.text( Voltage[i].Voltage)));___v1ew.push("\n");
___v1ew.push("        </option>\n");
___v1ew.push("        ");};___v1ew.push("</select>\n");
___v1ew.push("      <input name=\"special-primary-dw\" id=\"special-primary-delta\" class=\"voltage-type\" type=\"radio\" value=\"D\" checked=\"checked\" />\n");
___v1ew.push("      <label for=\"special-primary-delta\">\n");
___v1ew.push("        Delta\n");
___v1ew.push("      </label>\n");
___v1ew.push("      <input name=\"special-primary-dw\" id=\"special-primary-wye\" class=\"voltage-type\" type=\"radio\" value=\"Y\" />\n");
___v1ew.push("      <label for=\"special-primary-wye\">\n");
___v1ew.push("        Wye\n");
___v1ew.push("      </label>\n");
___v1ew.push("    </li>\n");
___v1ew.push("    <li>\n");
___v1ew.push("      <label for=\"special-secondary-voltage\" class=\"master\">\n");
___v1ew.push("        Secondary Voltage\n");
___v1ew.push("      </label>\n");
___v1ew.push("      <select id=\"special-secondary-voltage\" class=\"required\">\n");
___v1ew.push("        <option value=\"-1\"></option>\n");
___v1ew.push("        ");for(var i = 0; i < Voltage.length ; i++){;___v1ew.push("\n");
___v1ew.push("        <option value=\"");___v1ew.push((jQuery.EJS.text( Voltage[i].Voltage)));___v1ew.push("\">");___v1ew.push((jQuery.EJS.text( Voltage[i].Voltage)));___v1ew.push("\n");
___v1ew.push("        </option>\n");
___v1ew.push("        ");};___v1ew.push("</select>\n");
___v1ew.push("      <input name=\"special-secondary-dw\" id=\"special-secondary-delta\" class=\"voltage-type\" type=\"radio\" value=\"D\"  checked=\"checked\"/>\n");
___v1ew.push("      <label for=\"special-secondary-delta\">\n");
___v1ew.push("        Delta\n");
___v1ew.push("      </label>\n");
___v1ew.push("      <input name=\"special-secondary-dw\" id=\"special-secondary-wye\" class=\"voltage-type\" type=\"radio\" value=\"Y\" />\n");
___v1ew.push("      <label for=\"special-secondary-wye\">\n");
___v1ew.push("        Wye\n");
___v1ew.push("      </label>\n");
___v1ew.push("    </li>\n");
___v1ew.push("\n");
___v1ew.push("    <li class='clear'>\n");
___v1ew.push("      <label for=\"special-taps\" class=\"master\">\n");
___v1ew.push("        Taps\n");
___v1ew.push("      </label>\n");
___v1ew.push("      <select id=\"special-taps\" class=\"required\">\n");
___v1ew.push("        <option value=\"-1\"></option>\n");
___v1ew.push("        ");for(var i = 0; i < Taps.length ; i++){;___v1ew.push("\n");
___v1ew.push("            <option value=\"");___v1ew.push((jQuery.EJS.text( Taps[i].TapNumber)));___v1ew.push("\">\n");
___v1ew.push("                ");___v1ew.push((jQuery.EJS.text( Taps[i].TapName)));___v1ew.push(" \n");
___v1ew.push("                ");if (Taps[i].TapName == "2-2½% FCAN and 4-2½% FCBN"){;___v1ew.push("\n");
___v1ew.push("                    (Standard)\n");
___v1ew.push("                ");};___v1ew.push("\n");
___v1ew.push("            </option>\n");
___v1ew.push("        ");};___v1ew.push("\n");
___v1ew.push("      </select>\n");
___v1ew.push("    </li>\n");
___v1ew.push("  </ul>\n");
___v1ew.push("\n");
___v1ew.push("  <ul id=\"specialkit\" class='kit'>\n");
___v1ew.push("    <li class=\"double clear\">\n");
___v1ew.push("      <h3>Accesories</h3>\n");
___v1ew.push("      <div id=\"specialwall\" class=\"accessories\">\n");
___v1ew.push("        <label for=\"special-wall\">\n");
___v1ew.push("          Wall Bracket\n");
___v1ew.push("        </label>\n");
___v1ew.push("      </div>\n");
___v1ew.push("      <div id=\"specialrain\" class=\"accessories\">\n");
___v1ew.push("        <label for=\"special-accessories-rain\">\n");
___v1ew.push("          Rain Hood\n");
___v1ew.push("        </label>\n");
___v1ew.push("      </div>\n");
___v1ew.push("        <div id=\"specialnone\" class=\"accessories\">\n");
___v1ew.push("            <label for=\"special-accessories-none\">\n");
___v1ew.push("                This item is already rated Indoor/Outdoor or not eligible for a wall bracket.\n");
___v1ew.push("            </label>\n");
___v1ew.push("        </div>\n");
___v1ew.push("    </li>\n");
___v1ew.push("    <li class=\"id\">\n");
___v1ew.push("      <label for=\"special-kit-price\" class=\"master\">\n");
___v1ew.push("        Kit Price\n");
___v1ew.push("      </label>\n");
___v1ew.push("      <span id=\"special-kit-price\"></span>\n");
___v1ew.push("    </li>\n");
___v1ew.push("    <li class=\"id\">\n");
___v1ew.push("      <label for=\"special-kit-quantity\" class=\"master\">Qty</label>\n");
___v1ew.push("      <input type=\"button\" class=\"subtract quantity-btn\" value=\"-\">/<input type=\"button\" class=\"add quantity-btn\" value=\"+\"><input type=\"text\" id=\"special-kit-quantity\" value=\"0\"  class='quantity disabled' disabled/>\n");
___v1ew.push("      <input type='hidden' id='specialkitID'/>\n");
___v1ew.push("    </li>\n");
___v1ew.push("  </ul>\n");
___v1ew.push("  <input type='hidden' id='specialID' value=\"0\"/>\n");
___v1ew.push("  <input id=\"special-add\" type=\"button\" value=\"Add to Quote\"  class=\"action\"/>\n");
___v1ew.push("</div>\n");
___v1ew.push("<div class=\"buttons\">\n");
___v1ew.push("  <input id=\"btn-gotoquote\" type=\"button\" class=\"btnadd action\" value=\"Go to Quote\"/>\n");
___v1ew.push("</div>");; return ___v1ew.join('');}}}catch(e){e.lineNumber=null;throw e;} }));;
steal.end();
$.View.preload('mgmtransformer_views_newquote_kva_ejs',jQuery.EJS(function(_CONTEXT,_VIEW) { try { with(_VIEW) { with (_CONTEXT) {var ___v1ew = [];___v1ew.push("﻿<option value=\"-1\"></option>\n");
for(var i = 0; i < kvas.length ; i++){;___v1ew.push("\n");
___v1ew.push("<option value=\"");___v1ew.push((jQuery.EJS.text( kvas[i].kva)));___v1ew.push("\">");___v1ew.push((jQuery.EJS.text( kvas[i].kva)));___v1ew.push("\n");
___v1ew.push("</option>\n");
};; return ___v1ew.join('');}}}catch(e){e.lineNumber=null;throw e;} }));;
steal.end();
$.View.preload('mgmtransformer_views_newquote_phase_ejs',jQuery.EJS(function(_CONTEXT,_VIEW) { try { with(_VIEW) { with (_CONTEXT) {var ___v1ew = [];___v1ew.push("<label class=\"master\">Phase</label>\n");
for(var i = 0; i < phases.length ; i++){;___v1ew.push("\n");
___v1ew.push("	"); if (phases[i].phase == "Single") {;___v1ew.push("    \n");
___v1ew.push("        <input name=\"stock-phase\" id=\"stock-phase-single\" type=\"radio\" value=\"single\" />\n");
___v1ew.push("        <label for=\"stock-phase-single\">\n");
___v1ew.push("            Single Phase\n");
___v1ew.push("        </label>\n");
___v1ew.push("    ");}else{;___v1ew.push("    \n");
___v1ew.push("        <input name=\"stock-phase\" id=\"stock-phase-three\" type=\"radio\" value=\"three\" checked=\"checked\"/>\n");
___v1ew.push("        <label for=\"stock-phase-three\">\n");
___v1ew.push("            Three Phase\n");
___v1ew.push("        </label>\n");
___v1ew.push("    ");};___v1ew.push("        \n");
};; return ___v1ew.join('');}}}catch(e){e.lineNumber=null;throw e;} }));;
steal.end();
$.View.preload('mgmtransformer_views_quote_edit_ejs',jQuery.EJS(function(_CONTEXT,_VIEW) { try { with(_VIEW) { with (_CONTEXT) {var ___v1ew = [];for(var attribute in Mgmtransformer.Models.Quote.attributes){;___v1ew.push("\n");
___v1ew.push("	");if(attribute == 'id') continue;;___v1ew.push("\n");
___v1ew.push("	<td class='");___v1ew.push((jQuery.EJS.text( attribute )));___v1ew.push("'>\n");
___v1ew.push("		<input type=\"text\" value=\"");___v1ew.push((jQuery.EJS.text( this[attribute])));___v1ew.push("\" name=\"");___v1ew.push((jQuery.EJS.text( attribute)));___v1ew.push("\"/>\n");
___v1ew.push("	</td>\n");
};___v1ew.push("\n");
___v1ew.push("<td>\n");
___v1ew.push("	<input type='submit' value='Update' class='update'/>\n");
___v1ew.push("	<a href='javascript://' class='cancel'>cancel</a>\n");
___v1ew.push("</td>");; return ___v1ew.join('');}}}catch(e){e.lineNumber=null;throw e;} }));;
steal.end();
$.View.preload('mgmtransformer_views_quote_init_ejs',jQuery.EJS(function(_CONTEXT,_VIEW) { try { with(_VIEW) { with (_CONTEXT) {var ___v1ew = [];___v1ew.push("<div class=\"quotelist\">\n");
___v1ew.push("  <h2>Quote History</h2>\n");
___v1ew.push("  <table cellspacing='0px'>\n");
___v1ew.push("	  <thead>\n");
___v1ew.push("	  <tr>\n");
___v1ew.push("      <th>quote number</th>\n");
___v1ew.push("      <th>job name</th>\n");
___v1ew.push("      <th>date</th>\n");
___v1ew.push("		  <th>options</th>\n");
___v1ew.push("	  </tr>\n");
___v1ew.push("	  </thead>\n");
___v1ew.push("	  <tbody>\n");
___v1ew.push("		  ");___v1ew.push((jQuery.EJS.text( $.View('//mgmtransformer/views/quote/list',{quotes: quotes}))));___v1ew.push("\n");
___v1ew.push("	  </tbody>\n");
___v1ew.push("  </table>\n");
___v1ew.push("</div>\n");
___v1ew.push("<div class='quoteshow'>\n");
___v1ew.push("  <h2>\n");
___v1ew.push("    Quote# <span>####-##</span>\n");
___v1ew.push("  </h2>\n");
___v1ew.push("  <div class='quotecustomer'> </div>\n");
___v1ew.push("  <div class='quotedetaillist'> </div>\n");
___v1ew.push("  <div class=\"buttons\">\n");
___v1ew.push("      <input id=\"btn-history\" type=\"button\" class=\"btnadd return\" value=\"Back to History\" />\n");
___v1ew.push("      <input id=\"btn-copyquote\" type=\"button\" class=\"btnadd\" value=\"Copy Quote\" />\n");
___v1ew.push("  </div>\n");
___v1ew.push("</div>\n");
___v1ew.push("<div class='quoteedit'>\n");
___v1ew.push("  <h2>\n");
___v1ew.push("    Quote# <span>####-##</span>\n");
___v1ew.push("  </h2>\n");
___v1ew.push("  <div class='quotecustomer'> </div>\n");
___v1ew.push("  <div class='quotedetaillist'> </div>\n");
___v1ew.push("  <div class=\"buttons\">\n");
___v1ew.push("      <input id=\"btn-addquote\" type=\"button\" class=\"btnadd return\" value=\"Add Quote Item\" />\n");
___v1ew.push("      <input id=\"btn-customer\" type=\"button\" class=\"btnadd\" value=\"Add Customer Info\" />\n");
___v1ew.push("      <input id=\"btn-summary\" type=\"button\" class=\"btnadd\" value=\"Back to Summary\" />\n");
___v1ew.push("      <input id=\"btn-finalquote\" type=\"button\" class=\"btnadd\" value=\"Finalize Quote\" />\n");
___v1ew.push("  </div>\n");
___v1ew.push("</div>\n");
___v1ew.push("\n");
; return ___v1ew.join('');}}}catch(e){e.lineNumber=null;throw e;} }));;
steal.end();
$.View.preload('mgmtransformer_views_quote_list_ejs',jQuery.EJS(function(_CONTEXT,_VIEW) { try { with(_VIEW) { with (_CONTEXT) {var ___v1ew = [];for(var i = 0; i < quotes.length ; i++){;___v1ew.push("\n");
___v1ew.push("	<tr ");___v1ew.push((jQuery.EJS.text( quotes[i])));___v1ew.push(">\n");
___v1ew.push("		");___v1ew.push((jQuery.EJS.text( $.View('//mgmtransformer/views/quote/show',quotes[i]))));___v1ew.push("\n");
___v1ew.push("	</tr>\n");
};; return ___v1ew.join('');}}}catch(e){e.lineNumber=null;throw e;} }));;
steal.end();
$.View.preload('mgmtransformer_views_quote_show_ejs',jQuery.EJS(function(_CONTEXT,_VIEW) { try { with(_VIEW) { with (_CONTEXT) {var ___v1ew = [];___v1ew.push("<td class='quoteid'>");___v1ew.push((jQuery.EJS.text(this['quoteid'])));___v1ew.push("</td>\n");
___v1ew.push("<td class='jobname'>");___v1ew.push((jQuery.EJS.text(this['jobname'])));___v1ew.push("</td>\n");
___v1ew.push("<td class='createdate'>");___v1ew.push((jQuery.EJS.text(this['createdate'])));___v1ew.push("</td>\n");
___v1ew.push("<td class='options'>\n");
___v1ew.push("	<a href='javascript://' class='show'>show</a>\n");
___v1ew.push("  <a href='/MGMQuotation/pdfs/");___v1ew.push((jQuery.EJS.text(this["pdfUrl"])));___v1ew.push("' class='pdf' target=\"_quotepdf\">pdf</a>\n");
___v1ew.push("</td>");; return ___v1ew.join('');}}}catch(e){e.lineNumber=null;throw e;} }));;
steal.end();
$.View.preload('mgmtransformer_views_quotedetail_edit_ejs',jQuery.EJS(function(_CONTEXT,_VIEW) { try { with(_VIEW) { with (_CONTEXT) {var ___v1ew = [];___v1ew.push("<div id=\"newquote\" class=\"tab\">\n");
___v1ew.push("        <div id=\"stock-unit\">\n");
___v1ew.push("            <h3>\n");
___v1ew.push("                Standard/Stock Unit</h3>\n");
___v1ew.push("            <fieldset>\n");
___v1ew.push("                <legend>Phase</legend>\n");
___v1ew.push("                <input name=\"stock-phase\" id=\"stock-phase-single\" type=\"radio\" value=\"single\" />\n");
___v1ew.push("                <label for=\"stock-phase-single\">\n");
___v1ew.push("                    Single Phase</label>\n");
___v1ew.push("                <input name=\"stock-phase\" id=\"stock-phase-three\" type=\"radio\" value=\"three\" />\n");
___v1ew.push("                <label for=\"stock-phase-three\">\n");
___v1ew.push("                    Three Phase</label>\n");
___v1ew.push("            </fieldset>\n");
___v1ew.push("            <label for=\"stock-kva\">\n");
___v1ew.push("                KVA</label>\n");
___v1ew.push("            <select id=\"stock-kva\">\n");
___v1ew.push("                <option>75</option>\n");
___v1ew.push("                <option>85</option>\n");
___v1ew.push("                <option>95</option>\n");
___v1ew.push("            </select>\n");
___v1ew.push("            <fieldset>\n");
___v1ew.push("                <legend>Windings</legend>\n");
___v1ew.push("                <input name=\"stock-windings\" id=\"stock-windings-aluminum\" type=\"radio\" value=\"aluminum\" />\n");
___v1ew.push("                <label for=\"stock-windings-aluminum\">\n");
___v1ew.push("                    Aluminum</label>\n");
___v1ew.push("                <input name=\"stock-windings\" id=\"stock-windings-copper\" type=\"radio\" value=\"copper\" />\n");
___v1ew.push("                <label for=\"stock-windings-copper\">\n");
___v1ew.push("                    Copper</label>\n");
___v1ew.push("            </fieldset>\n");
___v1ew.push("            <label for=\"stock-configuration\">\n");
___v1ew.push("                Configuration</label>\n");
___v1ew.push("            <select id=\"stock-configuration\">\n");
___v1ew.push("                <option>480 - 208Y/120(YS)</option>\n");
___v1ew.push("                <option>480 - 208Y/120(YS)</option>\n");
___v1ew.push("                <option>480 - 208Y/120(YS)</option>\n");
___v1ew.push("            </select>\n");
___v1ew.push("            <label for=\"stock-quantity\">\n");
___v1ew.push("                Qty</label>\n");
___v1ew.push("            <input type=\"text\" id=\"stock-quantity\" value=\"0\" />\n");
___v1ew.push("            <label for=\"stock-catalog\">\n");
___v1ew.push("                Catalog No.</label>\n");
___v1ew.push("            <input type=\"text\" id=\"stock-catalog\" value=\"\" />\n");
___v1ew.push("            <input id=\"stock-add\" type=\"button\" value=\"Add to Quote\" />\n");
___v1ew.push("        </div>\n");
___v1ew.push("\n");
___v1ew.push("        <div id=\"special-unit\">\n");
___v1ew.push("            <h3>\n");
___v1ew.push("                Special Configuration</h3>\n");
___v1ew.push("            <label for=\"special-quantity\">\n");
___v1ew.push("                Qty</label>\n");
___v1ew.push("            <input type=\"text\" id=\"special-quantity\" value=\"0\" />\n");
___v1ew.push("            <label for=\"special-kva\">\n");
___v1ew.push("                KVA</label>\n");
___v1ew.push("            <select id=\"special-kva\">\n");
___v1ew.push("                <option>75</option>\n");
___v1ew.push("                <option>85</option>\n");
___v1ew.push("                <option>95</option>\n");
___v1ew.push("            </select>\n");
___v1ew.push("            <label for=\"special-temp\">\n");
___v1ew.push("                Temp Rise</label>\n");
___v1ew.push("            <select id=\"special-temp\">\n");
___v1ew.push("                <option>75</option>\n");
___v1ew.push("                <option>85</option>\n");
___v1ew.push("                <option>95</option>\n");
___v1ew.push("            </select>\n");
___v1ew.push("\n");
___v1ew.push("            <label for=\"special-primary-voltage\">\n");
___v1ew.push("                Primary Voltage</label>\n");
___v1ew.push("            <select id=\"special-primary-voltage\">\n");
___v1ew.push("                <option>75</option>\n");
___v1ew.push("                <option>85</option>\n");
___v1ew.push("                <option>95</option>\n");
___v1ew.push("            </select>\n");
___v1ew.push("            <input name=\"special-primary-dw\" id=\"special-primary-delta\" type=\"radio\" value=\"delta\" />\n");
___v1ew.push("            <label for=\"special-primary-delta\">\n");
___v1ew.push("                Delta</label>\n");
___v1ew.push("            <input name=\"special-primary-dw\" id=\"special-primary-wye\" type=\"radio\" value=\"wye\" />\n");
___v1ew.push("            <label for=\"special-primary-wye\">\n");
___v1ew.push("                Wye</label>\n");
___v1ew.push("\n");
___v1ew.push("            <label for=\"special-secondary-voltage\">\n");
___v1ew.push("                Secondary Voltage</label>\n");
___v1ew.push("            <select id=\"special-secondary-voltage\">\n");
___v1ew.push("                <option>75</option>\n");
___v1ew.push("                <option>85</option>\n");
___v1ew.push("                <option>95</option>\n");
___v1ew.push("            </select>\n");
___v1ew.push("            <input name=\"special-secondary-dw\" id=\"special-secondary-delta\" type=\"radio\" value=\"delta\" />\n");
___v1ew.push("            <label for=\"special-secondary-delta\">\n");
___v1ew.push("                Delta</label>\n");
___v1ew.push("            <input name=\"special-secondary-dw\" id=\"special-secondary-wye\" type=\"radio\" value=\"wye\" />\n");
___v1ew.push("            <label for=\"special-secondary-wye\">\n");
___v1ew.push("                Wye</label>\n");
___v1ew.push("\n");
___v1ew.push("            <label for=\"special-kfactor\">\n");
___v1ew.push("                K-Factor</label>\n");
___v1ew.push("            <select id=\"special-kfactor\">\n");
___v1ew.push("                <option>75</option>\n");
___v1ew.push("                <option>85</option>\n");
___v1ew.push("                <option>95</option>\n");
___v1ew.push("            </select>\n");
___v1ew.push("\n");
___v1ew.push("            <label for=\"special-taps\">\n");
___v1ew.push("                Taps</label>\n");
___v1ew.push("            <select id=\"special-taps\">\n");
___v1ew.push("                <option>75</option>\n");
___v1ew.push("                <option>85</option>\n");
___v1ew.push("                <option>95</option>\n");
___v1ew.push("            </select>\n");
___v1ew.push("\n");
___v1ew.push("            <label for=\"special-sound\">\n");
___v1ew.push("                Sound Level</label>\n");
___v1ew.push("            <select id=\"special-sound\">\n");
___v1ew.push("                <option>75</option>\n");
___v1ew.push("                <option>85</option>\n");
___v1ew.push("                <option>95</option>\n");
___v1ew.push("            </select>\n");
___v1ew.push("\n");
___v1ew.push("            <fieldset>\n");
___v1ew.push("                <legend>Windings</legend>\n");
___v1ew.push("                <input name=\"special-windings\" id=\"special-windings-aluminum\" type=\"radio\" value=\"aluminum\" />\n");
___v1ew.push("                <label for=\"special-windings-aluminum\">\n");
___v1ew.push("                    Aluminum</label>\n");
___v1ew.push("                <input name=\"special-windings\" id=\"special-windings-copper\" type=\"radio\" value=\"copper\" />\n");
___v1ew.push("                <label for=\"special-windings-copper\">\n");
___v1ew.push("                    Copper</label>\n");
___v1ew.push("            </fieldset>\n");
___v1ew.push("\n");
___v1ew.push("            <fieldset>\n");
___v1ew.push("                <legend>Phase</legend>\n");
___v1ew.push("                <input name=\"special-phase\" id=\"special-phase-single\" type=\"radio\" value=\"single\" />\n");
___v1ew.push("                <label for=\"special-phase-single\">\n");
___v1ew.push("                    Single Phase</label>\n");
___v1ew.push("                <input name=\"special-phase\" id=\"special-phase-three\" type=\"radio\" value=\"three\" />\n");
___v1ew.push("                <label for=\"special-phase-three\">\n");
___v1ew.push("                    Three Phase</label>\n");
___v1ew.push("            </fieldset>\n");
___v1ew.push("\n");
___v1ew.push("            <input id=\"special-add\" type=\"button\" value=\"Add to Quote\" />\n");
___v1ew.push("        </div>\n");
___v1ew.push("\n");
___v1ew.push("        <div id=\"accessories\">\n");
___v1ew.push("            <h3>Accesories</h3>\n");
___v1ew.push("            <input id=\"accessories-rain\" type=\"checkbox\" value=\"rain\" />\n");
___v1ew.push("            <label for=\"accessories-rain\">\n");
___v1ew.push("                Rain Hood</label>\n");
___v1ew.push("\n");
___v1ew.push("            <label for=\"accessories-rain-quantity\">Qty</label>\n");
___v1ew.push("            <input type=\"text\" id=\"accessories-rain-quantity\" value=\"0\" />\n");
___v1ew.push("\n");
___v1ew.push("            <input id=\"accessories-wall\" type=\"checkbox\" value=\"wall\" />\n");
___v1ew.push("            <label for=\"accessories-wall\">\n");
___v1ew.push("                Wall Bracket</label>\n");
___v1ew.push("\n");
___v1ew.push("            <label for=\"accessories-wall-quantity\">Qty</label>\n");
___v1ew.push("            <input type=\"text\" id=\"accessories-wall-quantity\" value=\"0\" />\n");
___v1ew.push("\n");
___v1ew.push("            <input id=\"accessories-add\" type=\"button\" value=\"Add to Quote\" />\n");
___v1ew.push("        </div>\n");
___v1ew.push("    </div>");; return ___v1ew.join('');}}}catch(e){e.lineNumber=null;throw e;} }));;
steal.end();
$.View.preload('mgmtransformer_views_quotedetail_init_ejs',jQuery.EJS(function(_CONTEXT,_VIEW) { try { with(_VIEW) { with (_CONTEXT) {var ___v1ew = [];___v1ew.push("<table class=\"transformers\">\n");
___v1ew.push("  <thead>\n");
___v1ew.push("    <tr>\n");
___v1ew.push("      <th>kva</th>\n");
___v1ew.push("      <th>ph</th>\n");
___v1ew.push("      <th>pri. volts</th>\n");
___v1ew.push("      <th>sec. volts</th>\n");
___v1ew.push("      <th>&deg;C rise</th>\n");
___v1ew.push("      <th>hz</th>\n");
___v1ew.push("      <th>wdg.mtl.</th>\n");
___v1ew.push("      <th>es</th>\n");
___v1ew.push("      <th>enclosure type</th>\n");
___v1ew.push("      <th>sound level</th>\n");
___v1ew.push("      <th>tp1</th>\n");
___v1ew.push("      <th>kf</th>\n");
___v1ew.push("      <th>price ea. $</th>\n");
___v1ew.push("      <th>total price $</th>\n");
___v1ew.push("      <th>catalog#</th>\n");
___v1ew.push("      <th>qty</th>\n");
___v1ew.push("      <th></th>\n");
___v1ew.push("    </tr>\n");
___v1ew.push("  </thead>\n");
___v1ew.push("  <tbody>\n");
___v1ew.push("    ");___v1ew.push((jQuery.EJS.text( $.View('//mgmtransformer/views/quotedetail/list',{quotedetails: quotedetails, isHistory: isHistory}))));___v1ew.push("\n");
___v1ew.push("    </tbody>\n");
___v1ew.push("</table>\n");
___v1ew.push("\n");
 if (haskit) {;___v1ew.push("\n");
___v1ew.push("<table class=\"accessories\">\n");
___v1ew.push("    <thead>\n");
___v1ew.push("        <tr>\n");
___v1ew.push("            <th>kit</th>\n");
___v1ew.push("            <th>price ea. $</th>\n");
___v1ew.push("            <th>total price $</th>\n");
___v1ew.push("            <th>qty</th>\n");
___v1ew.push("            <th></th>\n");
___v1ew.push("        </tr>\n");
___v1ew.push("    </thead>\n");
___v1ew.push("    <tbody>\n");
___v1ew.push("        ");___v1ew.push((jQuery.EJS.text( $.View('//mgmtransformer/views/quotedetail/kitlist',{quotedetails: quotedetails, isHistory: isHistory}))));___v1ew.push("\n");
___v1ew.push("    </tbody>\n");
___v1ew.push("</table>\n");
};___v1ew.push("\n");
___v1ew.push("\n");
___v1ew.push("\n");
___v1ew.push("<div id='total'>total: $<span id='totalprice'></span></div>\n");
___v1ew.push("\n");
; return ___v1ew.join('');}}}catch(e){e.lineNumber=null;throw e;} }));;
steal.end();
$.View.preload('mgmtransformer_views_quotedetail_list_ejs',jQuery.EJS(function(_CONTEXT,_VIEW) { try { with(_VIEW) { with (_CONTEXT) {var ___v1ew = [];for(var i = 0; i < quotedetails.length ; i++){
	if (quotedetails[i].stockUnitID != 0 || quotedetails[i].customStockID != 0) {;___v1ew.push("\n");
___v1ew.push("	<tr ");___v1ew.push((jQuery.EJS.text( quotedetails[i])));___v1ew.push(" class=\"quotedetail\">\n");
___v1ew.push("		");___v1ew.push((jQuery.EJS.text( $.View('//mgmtransformer/views/quotedetail/show',{quotedetail:quotedetails[i], isHistory:isHistory}))));___v1ew.push("\n");
___v1ew.push("	</tr>\n");
___v1ew.push("	");}
};___v1ew.push("\n");
___v1ew.push("\n");
; return ___v1ew.join('');}}}catch(e){e.lineNumber=null;throw e;} }));;
steal.end();
$.View.preload('mgmtransformer_views_quotedetail_show_ejs',jQuery.EJS(function(_CONTEXT,_VIEW) { try { with(_VIEW) { with (_CONTEXT) {var ___v1ew = [];if (quotedetail.stockUnitID != 0) {;___v1ew.push("\n");
___v1ew.push("\n");
___v1ew.push("  <td class='stockKVA'>");___v1ew.push((jQuery.EJS.text( quotedetail.stockKVA)));___v1ew.push("</td>\n");
___v1ew.push("  <td class='stockPhase'>");___v1ew.push((jQuery.EJS.text( quotedetail.stockPhase)));___v1ew.push("</td>\n");
___v1ew.push("  <td class='primaryvoltage'>");___v1ew.push((jQuery.EJS.text( quotedetail.primaryvoltage)));___v1ew.push("</td>\n");
___v1ew.push("  <td class='secondaryvoltage'>");___v1ew.push((jQuery.EJS.text( quotedetail.secondaryvoltage)));___v1ew.push("</td>\n");
___v1ew.push("  <td class='temperature'>");___v1ew.push((jQuery.EJS.text( quotedetail.temperature)));___v1ew.push("</td>\n");
___v1ew.push("  <td class='hertz'>");___v1ew.push((jQuery.EJS.text( quotedetail.hertz)));___v1ew.push("</td>\n");
___v1ew.push("  <td class='stockWindings'>");___v1ew.push((jQuery.EJS.text( quotedetail.stockWindings)));___v1ew.push("</td>\n");
___v1ew.push("  <td class='esstandard'>");___v1ew.push((jQuery.EJS.text( quotedetail.esstandard)));___v1ew.push("</td>\n");
___v1ew.push("  <td class='enclosure'>");___v1ew.push((jQuery.EJS.text( quotedetail.enclosure)));___v1ew.push("</td>\n");
___v1ew.push("  <td class='soundLevel'>");___v1ew.push((jQuery.EJS.text( quotedetail.soundLevel)));___v1ew.push("</td>\n");
___v1ew.push("  <td class='tp1'>");___v1ew.push((jQuery.EJS.text( quotedetail.tp1)));___v1ew.push("</td>\n");
___v1ew.push("  <td class='kfactor' nowrap>");___v1ew.push((jQuery.EJS.text( quotedetail.stockKFactor)));___v1ew.push("</td>\n");
___v1ew.push("  <td class='price'>$");___v1ew.push((jQuery.EJS.text( quotedetail.price)));___v1ew.push("</td>\n");
___v1ew.push("  <td class='totalprice'></td>\n");
___v1ew.push("  <td class='stockCatalogNumber'>");___v1ew.push((jQuery.EJS.text( quotedetail.stockCatalogNumber)));___v1ew.push("</td>\n");
___v1ew.push("\n");
} else if (quotedetail.customStockID != 0) {;___v1ew.push("\n");
___v1ew.push("\n");
___v1ew.push("  <td class='customStockKVA'>");___v1ew.push((jQuery.EJS.text( quotedetail.customStockKVA)));___v1ew.push("</td>\n");
___v1ew.push("  <td class='customStockPhase'>");___v1ew.push((jQuery.EJS.text( quotedetail.customStockPhase)));___v1ew.push("</td>\n");
___v1ew.push("  <td class='customPrimaryVoltage'>");___v1ew.push((jQuery.EJS.text( quotedetail.formattedprimaryvoltage())));___v1ew.push("</td>\n");
___v1ew.push("  <td class='customSecondaryVoltage'>");___v1ew.push((jQuery.EJS.text( quotedetail.formattedsecondaryvoltage())));___v1ew.push("</td>\n");
___v1ew.push("  <td class='customStockTemperature'>");___v1ew.push((jQuery.EJS.text( quotedetail.customStockTemperature)));___v1ew.push("</td>\n");
___v1ew.push("  <td class='hz'>");___v1ew.push((jQuery.EJS.text( quotedetail.customStockHz)));___v1ew.push("</td>\n");
___v1ew.push("  <td class='customStockWindings'>");___v1ew.push((jQuery.EJS.text( quotedetail.customStockWindings)));___v1ew.push("</td>\n");
___v1ew.push("  <td class='customES'>");___v1ew.push((jQuery.EJS.text( quotedetail.customES)));___v1ew.push("</td>\n");
___v1ew.push("  <td class='customEnclosure'>");___v1ew.push((jQuery.EJS.text( quotedetail.customEnclosure)));___v1ew.push("</td>\n");
___v1ew.push("  <td class='customStockSoundLevels'>");___v1ew.push((jQuery.EJS.text( quotedetail.customStockSoundLevels)));___v1ew.push("</td>\n");
___v1ew.push("  <td class='customTp1'>");___v1ew.push((jQuery.EJS.text( quotedetail.customTp1)));___v1ew.push("</td>\n");
___v1ew.push("  <td class='customStockKFactor' nowrap>");___v1ew.push((jQuery.EJS.text( quotedetail.customStockKFactor)));___v1ew.push("</td>\n");
___v1ew.push("  <td class='price'>$");___v1ew.push((jQuery.EJS.text( quotedetail.customStockPrice)));___v1ew.push("</td>\n");
___v1ew.push("  <td class='totalprice'></td>\n");
___v1ew.push("  <td class='customCatalognumber'>");___v1ew.push((jQuery.EJS.text( quotedetail.customCatalogNumber)));___v1ew.push("</td>\n");
___v1ew.push("  \n");
};___v1ew.push("\n");
___v1ew.push("\n");
if (!isHistory) {;___v1ew.push("\n");
___v1ew.push("	<td class='quantity'><input class='quantity' value='");___v1ew.push((jQuery.EJS.text( quotedetail.quantity)));___v1ew.push("'/></td>\n");
___v1ew.push("	<td nowrap><input type=\"button\" class=\"qtyupdate\" value='update'/> <input type=\"button\" class=\"remove\" value='X'/></td>\n");
} else {;___v1ew.push("\n");
___v1ew.push("	<td class='quantity'>");___v1ew.push((jQuery.EJS.text( quotedetail.quantity)));___v1ew.push("</td>\n");
___v1ew.push("	<td></td>\n");
} ;; return ___v1ew.join('');}}}catch(e){e.lineNumber=null;throw e;} }));;
steal.end();
$.View.preload('mgmtransformer_views_quotedetail_empty_ejs',jQuery.EJS(function(_CONTEXT,_VIEW) { try { with(_VIEW) { with (_CONTEXT) {var ___v1ew = [];___v1ew.push("﻿<div class=\"empty\">Cart Empty</div>");; return ___v1ew.join('');}}}catch(e){e.lineNumber=null;throw e;} }));;
steal.end();
$.View.preload('mgmtransformer_views_quotedetail_kitlist_ejs',jQuery.EJS(function(_CONTEXT,_VIEW) { try { with(_VIEW) { with (_CONTEXT) {var ___v1ew = [];___v1ew.push("﻿");for(var i = 0; i < quotedetails.length ; i++){
	if (quotedetails[i].kitID != 0 && quotedetails[i].customStockID == 0 && quotedetails[i].stockUnitID == 0) {;___v1ew.push("\n");
___v1ew.push("	<tr ");___v1ew.push((jQuery.EJS.text( quotedetails[i])));___v1ew.push(" class=\"quotedetail\">\n");
___v1ew.push("		");___v1ew.push((jQuery.EJS.text( $.View('//mgmtransformer/views/quotedetail/kitshow',{quotedetail:quotedetails[i], isHistory:isHistory}))));___v1ew.push("\n");
___v1ew.push("	</tr>\n");
___v1ew.push("	");}
};; return ___v1ew.join('');}}}catch(e){e.lineNumber=null;throw e;} }));;
steal.end();
$.View.preload('mgmtransformer_views_quotedetail_kitshow_ejs',jQuery.EJS(function(_CONTEXT,_VIEW) { try { with(_VIEW) { with (_CONTEXT) {var ___v1ew = [];___v1ew.push("﻿");if (quotedetail.kitID != 0) {;___v1ew.push("\n");
___v1ew.push("\n");
___v1ew.push("  <td class='kitNumber'>");___v1ew.push((jQuery.EJS.text( quotedetail.kitNumber)));___v1ew.push("</td>\n");
___v1ew.push("  <td class='kitPrice'>$");___v1ew.push((jQuery.EJS.text( quotedetail.kitPrice)));___v1ew.push("</td>\n");
___v1ew.push("  <td class='totalprice'></td>\n");
___v1ew.push("	");if (!isHistory) {;___v1ew.push("\n");
___v1ew.push("		<td class='quantity'><input class='quantity' value='");___v1ew.push((jQuery.EJS.text( quotedetail.quantity)));___v1ew.push("'/></td>\n");
___v1ew.push("		<td><input type=\"button\" class=\"qtyupdate\" value='update'/></td>\n");
___v1ew.push("	");} else {;___v1ew.push("\n");
___v1ew.push("		<td class='quantity'>");___v1ew.push((jQuery.EJS.text( quotedetail.quantity)));___v1ew.push("</td>\n");
___v1ew.push("		<td></td>\n");
___v1ew.push("	");} ;___v1ew.push("\n");
___v1ew.push("\n");
};___v1ew.push("\n");
___v1ew.push("\n");
; return ___v1ew.join('');}}}catch(e){e.lineNumber=null;throw e;} }));;
steal.end();
$.View.preload('mgmtransformer_views_stock_edit_ejs',jQuery.EJS(function(_CONTEXT,_VIEW) { try { with(_VIEW) { with (_CONTEXT) {var ___v1ew = [];for(var attribute in Mgmtransformer.Models.Stock.attributes){;___v1ew.push("\n");
___v1ew.push("	");if(attribute == 'id') continue;;___v1ew.push("\n");
___v1ew.push("	<td class='");___v1ew.push((jQuery.EJS.text( attribute )));___v1ew.push("'>\n");
___v1ew.push("		<input type=\"text\" value=\"");___v1ew.push((jQuery.EJS.text( this[attribute])));___v1ew.push("\" name=\"");___v1ew.push((jQuery.EJS.text( attribute)));___v1ew.push("\"/>\n");
___v1ew.push("	</td>\n");
};___v1ew.push("\n");
___v1ew.push("<td>\n");
___v1ew.push("	<input type='submit' value='Update' class='update'/>\n");
___v1ew.push("	<a href='javascript://' class='cancel'>cancel</a>\n");
___v1ew.push("</td>");; return ___v1ew.join('');}}}catch(e){e.lineNumber=null;throw e;} }));;
steal.end();
$.View.preload('mgmtransformer_views_stock_init_ejs',jQuery.EJS(function(_CONTEXT,_VIEW) { try { with(_VIEW) { with (_CONTEXT) {var ___v1ew = [];___v1ew.push("<h2>Stock Unit Prices</h2>\n");
___v1ew.push("<div>\n");
___v1ew.push("  <label for=\"configurations\">Configurations: </label>\n");
___v1ew.push("  <select id=\"configurations\">\n");
___v1ew.push("    ");for (var i =0; i < configurations.length; i++) {;___v1ew.push("\n");
___v1ew.push("      <option value=\"");___v1ew.push((jQuery.EJS.text( configurations[i].Windings)));___v1ew.push("||");___v1ew.push((jQuery.EJS.text( configurations[i].Phase)));___v1ew.push("||");___v1ew.push((jQuery.EJS.text( configurations[i].Configuration)));___v1ew.push("\">");___v1ew.push((jQuery.EJS.text( configurations[i].Windings)));___v1ew.push(" ");___v1ew.push((jQuery.EJS.text( configurations[i].Phase)));___v1ew.push(" Phase - ");___v1ew.push((jQuery.EJS.text( configurations[i].Configuration)));___v1ew.push("</option>\n");
___v1ew.push("    ");};___v1ew.push("\n");
___v1ew.push("  </select>\n");
___v1ew.push("</div>\n");
___v1ew.push("<div id=\"stock-table\"></div>");; return ___v1ew.join('');}}}catch(e){e.lineNumber=null;throw e;} }));;
steal.end();
$.View.preload('mgmtransformer_views_stock_list_ejs',jQuery.EJS(function(_CONTEXT,_VIEW) { try { with(_VIEW) { with (_CONTEXT) {var ___v1ew = [];___v1ew.push("<table>\n");
___v1ew.push("  <thead>\n");
___v1ew.push("    <tr class='group ");___v1ew.push((jQuery.EJS.text( stocks[0].windings)));___v1ew.push("'>\n");
___v1ew.push("      <th colspan='13'>\n");
___v1ew.push("        ");___v1ew.push((jQuery.EJS.text( stocks[0].phase)));___v1ew.push(" Phase<br/>\n");
___v1ew.push("        ");___v1ew.push((jQuery.EJS.text( stocks[0].primaryvoltage)));___v1ew.push(" Primary ");if (stocks[0].phase == 'Three'){;___v1ew.push("Delta");};___v1ew.push("- ");___v1ew.push((jQuery.EJS.text( stocks[0].secondaryvoltage)));___v1ew.push(", ");___v1ew.push((jQuery.EJS.text( stocks[0].hertz)));___v1ew.push(" Hz, \n");
___v1ew.push("            ");___v1ew.push((jQuery.EJS.text( stocks[0].temperature)));___v1ew.push(" &deg;C, ");___v1ew.push((jQuery.EJS.text( stocks[0].windings)));___v1ew.push(", ");___v1ew.push((jQuery.EJS.text( stocks[0].esstandard)));___v1ew.push("\n");
___v1ew.push("        </th>\n");
___v1ew.push("    </tr>\n");
___v1ew.push("    <tr>\n");
___v1ew.push("        <th>kva</th>\n");
___v1ew.push("        <th>catalog #</th>\n");
___v1ew.push("        <th>alt #</th>\n");
___v1ew.push("        <th>price</th>\n");
___v1ew.push("        <th>kit #</th>\n");
___v1ew.push("        <th>kit price</th>\n");
___v1ew.push("        <th>h</th>\n");
___v1ew.push("        <th>w</th>\n");
___v1ew.push("        <th>d</th>\n");
___v1ew.push("        <th>wt</th>\n");
___v1ew.push("        <th>case</th>\n");
___v1ew.push("        <th>enclosure</th>\n");
___v1ew.push("    </tr>\n");
___v1ew.push("  </thead>\n");
___v1ew.push("  <tbody>\n");
___v1ew.push("    ");for(var i = 0; i < stocks.length ; i++){;___v1ew.push("\n");
___v1ew.push("		<tr ");___v1ew.push((jQuery.EJS.text( stocks[i])));___v1ew.push(">\n");
___v1ew.push("			");___v1ew.push((jQuery.EJS.text( $.View('//mgmtransformer/views/stock/show',stocks[i]))));___v1ew.push("\n");
___v1ew.push("		</tr>\n");
___v1ew.push("	");};___v1ew.push("\n");
___v1ew.push("  </tbody>\n");
___v1ew.push("</table>\n");
___v1ew.push("<p>*Rain Hood kits (RH) are required for Outdoor (NEMA 3R) applications</p>\n");
; return ___v1ew.join('');}}}catch(e){e.lineNumber=null;throw e;} }));;
steal.end();
$.View.preload('mgmtransformer_views_stock_show_ejs',jQuery.EJS(function(_CONTEXT,_VIEW) { try { with(_VIEW) { with (_CONTEXT) {var ___v1ew = [];___v1ew.push("\n");
___v1ew.push("<td class='kva'>");___v1ew.push((jQuery.EJS.text(this['kva'])));___v1ew.push("</td>\n");
___v1ew.push("<td class='catalog'>");___v1ew.push((jQuery.EJS.text(this['catalog'])));___v1ew.push("</td>\n");
___v1ew.push("<td class='alt'>");___v1ew.push((jQuery.EJS.text(this['alt'])));___v1ew.push("</td>\n");
___v1ew.push("<td class='price'>$");___v1ew.push((jQuery.EJS.text(this['price'])));___v1ew.push("</td>\n");
___v1ew.push("<td class='kit'>");___v1ew.push((jQuery.EJS.text(this['kit'])));___v1ew.push("</td>\n");
___v1ew.push("<td class='kitprice'>"); if (this['kitprice']){;___v1ew.push("$");};___v1ew.push((jQuery.EJS.text(this['kitprice'])));___v1ew.push("</td>\n");
___v1ew.push("<td class='height'>");___v1ew.push((jQuery.EJS.text(this['height'])));___v1ew.push("</td>\n");
___v1ew.push("<td class='width'>");___v1ew.push((jQuery.EJS.text(this['width'])));___v1ew.push("</td>\n");
___v1ew.push("<td class='depth'>");___v1ew.push((jQuery.EJS.text(this['depth'])));___v1ew.push("</td>\n");
___v1ew.push("<td class='weight'>");___v1ew.push((jQuery.EJS.text(this['weight'])));___v1ew.push("</td>\n");
___v1ew.push("<td class='unitCase'>");___v1ew.push((jQuery.EJS.text(this['unitCase'])));___v1ew.push("</td>\n");
___v1ew.push("<td class='enclosure'>");___v1ew.push((jQuery.EJS.text(this['enclosure']))); if (this['enclosure'] == 'Indoor'){;___v1ew.push("*");};___v1ew.push("</td>");; return ___v1ew.join('');}}}catch(e){e.lineNumber=null;throw e;} }));;
steal.end();
$.View.preload('mgmtransformer_views_customer_init_ejs',jQuery.EJS(function(_CONTEXT,_VIEW) { try { with(_VIEW) { with (_CONTEXT) {var ___v1ew = [];___v1ew.push("﻿<div id=\"job\"></div>");; return ___v1ew.join('');}}}catch(e){e.lineNumber=null;throw e;} }));;
steal.end();
$.View.preload('mgmtransformer_views_customer_edit_ejs',jQuery.EJS(function(_CONTEXT,_VIEW) { try { with(_VIEW) { with (_CONTEXT) {var ___v1ew = [];___v1ew.push("﻿<form id=\"customerform\" ");___v1ew.push((jQuery.EJS.text(customer)));___v1ew.push(">\n");
___v1ew.push("    <input type=\"hidden\" id=\"quoteid\" name=\"quoteid\" value=\"");___v1ew.push((jQuery.EJS.text(quoteid)));___v1ew.push("\" />\n");
___v1ew.push("    <div class=\"requiredmessage\">**Job name required**</div>\n");
___v1ew.push("    <div id=\"job\">\n");
___v1ew.push("      <h4>Customer Info</h4>\n");
___v1ew.push("      <ul>\n");
___v1ew.push("        <li></li>\n");
___v1ew.push("        <li>\n");
___v1ew.push("          <label for=\"jobname\">jobname</label>\n");
___v1ew.push("          <input id=\"jobname\" name=\"jobname\" type=\"text\" class=\"required\" value=\"");___v1ew.push((jQuery.EJS.text(customer.jobname)));___v1ew.push("\" />\n");
___v1ew.push("        </li>\n");
___v1ew.push("        <li>\n");
___v1ew.push("          <label>contact name</label>\n");
___v1ew.push("          <input id=\"contactname\" name=\"contactname\" type=\"text\" class=\"\" value=\"");___v1ew.push((jQuery.EJS.text(customer.contactname)));___v1ew.push("\"/>\n");
___v1ew.push("        </li>\n");
___v1ew.push("        <li>\n");
___v1ew.push("          <label for=\"telephone\">telephone</label>\n");
___v1ew.push("          <input id=\"telephone\" name=\"telephone\" type=\"text\" class=\"phone\" value=\"");___v1ew.push((jQuery.EJS.text(customer.telephone)));___v1ew.push("\"/>\n");
___v1ew.push("        </li>\n");
___v1ew.push("        <li>\n");
___v1ew.push("          <label for=\"email\">email</label>\n");
___v1ew.push("          <input id=\"email\" name=\"email\" type=\"text\" class=\"email\" value=\"");___v1ew.push((jQuery.EJS.text(customer.email)));___v1ew.push("\"/>\n");
___v1ew.push("        </li>\n");
___v1ew.push("      </ul>\n");
___v1ew.push("    </div>\n");
___v1ew.push("\n");
___v1ew.push("    <div id=\"billing\">\n");
___v1ew.push("      <h4>Billing Address</h4>\n");
___v1ew.push("      <ul>\n");
___v1ew.push("        <li></li>\n");
___v1ew.push("        <li>\n");
___v1ew.push("          <label for=\"company\">company</label>\n");
___v1ew.push("          <input id=\"company\" name=\"company\" type=\"text\" class=\"\" value=\"");___v1ew.push((jQuery.EJS.text(customer.company)));___v1ew.push("\"/>\n");
___v1ew.push("        </li>\n");
___v1ew.push("        <li>\n");
___v1ew.push("          <label for=\"addressline1\">address 1</label>\n");
___v1ew.push("          <input id=\"addressline1\" name=\"addressline1\" type=\"text\" class=\"\" value=\"");___v1ew.push((jQuery.EJS.text(customer.addressline1)));___v1ew.push("\"/>\n");
___v1ew.push("        </li>\n");
___v1ew.push("        <li>\n");
___v1ew.push("          <label for=\"addressline2\">address 2</label>\n");
___v1ew.push("          <input id=\"addressline2\" name=\"addressline2\" type=\"text\" class=\"\" value=\"");___v1ew.push((jQuery.EJS.text(customer.addressline2)));___v1ew.push("\"/>\n");
___v1ew.push("        </li>\n");
___v1ew.push("        <li>\n");
___v1ew.push("          <label for=\"city\">city</label>\n");
___v1ew.push("          <input id=\"city\" name=\"city\" type=\"text\" class=\"\" value=\"");___v1ew.push((jQuery.EJS.text(customer.city)));___v1ew.push("\"/>\n");
___v1ew.push("        </li>\n");
___v1ew.push("        <li>\n");
___v1ew.push("          <label for=\"state\">state</label>\n");
___v1ew.push("          <select id=\"state\" name=\"state\" class=\"\">\n");
___v1ew.push("            <option value=\"\" selected=\"selected\">Select a State</option>\n");
___v1ew.push("            <option value=\"AL\">Alabama</option>\n");
___v1ew.push("            <option value=\"AK\">Alaska</option>\n");
___v1ew.push("            <option value=\"AZ\">Arizona</option>\n");
___v1ew.push("            <option value=\"AR\">Arkansas</option>\n");
___v1ew.push("            <option value=\"CA\">California</option>\n");
___v1ew.push("            <option value=\"CO\">Colorado</option>\n");
___v1ew.push("            <option value=\"CT\">Connecticut</option>\n");
___v1ew.push("            <option value=\"DE\">Delaware</option>\n");
___v1ew.push("            <option value=\"DC\">District Of Columbia</option>\n");
___v1ew.push("            <option value=\"FL\">Florida</option>\n");
___v1ew.push("            <option value=\"GA\">Georgia</option>\n");
___v1ew.push("            <option value=\"HI\">Hawaii</option>\n");
___v1ew.push("            <option value=\"ID\">Idaho</option>\n");
___v1ew.push("            <option value=\"IL\">Illinois</option>\n");
___v1ew.push("            <option value=\"IN\">Indiana</option>\n");
___v1ew.push("            <option value=\"IA\">Iowa</option>\n");
___v1ew.push("            <option value=\"KS\">Kansas</option>\n");
___v1ew.push("            <option value=\"KY\">Kentucky</option>\n");
___v1ew.push("            <option value=\"LA\">Louisiana</option>\n");
___v1ew.push("            <option value=\"ME\">Maine</option>\n");
___v1ew.push("            <option value=\"MD\">Maryland</option>\n");
___v1ew.push("            <option value=\"MA\">Massachusetts</option>\n");
___v1ew.push("            <option value=\"MI\">Michigan</option>\n");
___v1ew.push("            <option value=\"MN\">Minnesota</option>\n");
___v1ew.push("            <option value=\"MS\">Mississippi</option>\n");
___v1ew.push("            <option value=\"MO\">Missouri</option>\n");
___v1ew.push("            <option value=\"MT\">Montana</option>\n");
___v1ew.push("            <option value=\"NE\">Nebraska</option>\n");
___v1ew.push("            <option value=\"NV\">Nevada</option>\n");
___v1ew.push("            <option value=\"NH\">New Hampshire</option>\n");
___v1ew.push("            <option value=\"NJ\">New Jersey</option>\n");
___v1ew.push("            <option value=\"NM\">New Mexico</option>\n");
___v1ew.push("            <option value=\"NY\">New York</option>\n");
___v1ew.push("            <option value=\"NC\">North Carolina</option>\n");
___v1ew.push("            <option value=\"ND\">North Dakota</option>\n");
___v1ew.push("            <option value=\"OH\">Ohio</option>\n");
___v1ew.push("            <option value=\"OK\">Oklahoma</option>\n");
___v1ew.push("            <option value=\"OR\">Oregon</option>\n");
___v1ew.push("            <option value=\"PA\">Pennsylvania</option>\n");
___v1ew.push("            <option value=\"RI\">Rhode Island</option>\n");
___v1ew.push("            <option value=\"SC\">South Carolina</option>\n");
___v1ew.push("            <option value=\"SD\">South Dakota</option>\n");
___v1ew.push("            <option value=\"TN\">Tennessee</option>\n");
___v1ew.push("            <option value=\"TX\">Texas</option>\n");
___v1ew.push("            <option value=\"UT\">Utah</option>\n");
___v1ew.push("            <option value=\"VT\">Vermont</option>\n");
___v1ew.push("            <option value=\"VA\">Virginia</option>\n");
___v1ew.push("            <option value=\"WA\">Washington</option>\n");
___v1ew.push("            <option value=\"WV\">West Virginia</option>\n");
___v1ew.push("            <option value=\"WI\">Wisconsin</option>\n");
___v1ew.push("            <option value=\"WY\">Wyoming</option>\n");
___v1ew.push("          </select>\n");
___v1ew.push("        </li>\n");
___v1ew.push("        <li>\n");
___v1ew.push("          <label for=\"zip\">zip</label>\n");
___v1ew.push("          <input id=\"zip\" name=\"zip\" type=\"text\" class=\" zip\" value=\"");___v1ew.push((jQuery.EJS.text(customer.zip)));___v1ew.push("\"/>\n");
___v1ew.push("        </li>\n");
___v1ew.push("      </ul>\n");
___v1ew.push("    </div>\n");
___v1ew.push("\n");
___v1ew.push("    <div id=\"shipping\">\n");
___v1ew.push("      <h4>Shipping To</h4>\n");
___v1ew.push("      <ul>\n");
___v1ew.push("        <li>\n");
___v1ew.push("          <label for=\"same\">same as billing</label>\n");
___v1ew.push("          <input id=\"issamebilling\" name=\"issamebilling\" type=\"checkbox\" "); if (customer.issamebilling == "True") {;___v1ew.push("checked=\"checked\"");};___v1ew.push(" value=\"True\"/>\n");
___v1ew.push("        </li>\n");
___v1ew.push("        <li>\n");
___v1ew.push("          <label for=\"shipcompany\">company</label>\n");
___v1ew.push("          <input id=\"shipcompany\" name=\"shipcompany\" type=\"text\" disabled=\"disabled\" class=\"\" value=\"");___v1ew.push((jQuery.EJS.text(customer.shipcompany)));___v1ew.push("\"/>\n");
___v1ew.push("        </li>\n");
___v1ew.push("        <li>\n");
___v1ew.push("          <label for=\"shipaddressline1\">address 1</label>\n");
___v1ew.push("          <input id=\"shipaddressline1\" name=\"shipaddressline1\" type=\"text\" disabled=\"disabled\" class=\"\" value=\"");___v1ew.push((jQuery.EJS.text(customer.shipaddressline1)));___v1ew.push("\"/>\n");
___v1ew.push("        </li>\n");
___v1ew.push("        <li>\n");
___v1ew.push("          <label for=\"shipaddressline2\">address 2</label>\n");
___v1ew.push("          <input id=\"shipaddressline2\" name=\"shipaddressline2\" type=\"text\" disabled=\"disabled\" class=\"\" value=\"");___v1ew.push((jQuery.EJS.text(customer.shipaddressline2)));___v1ew.push("\"/>\n");
___v1ew.push("        </li>\n");
___v1ew.push("        <li>\n");
___v1ew.push("          <label for=\"shipcity\">city</label>\n");
___v1ew.push("          <input id=\"shipcity\" name=\"shipcity\" type=\"text\" disabled=\"disabled\" class=\"\" value=\"");___v1ew.push((jQuery.EJS.text(customer.shipcity)));___v1ew.push("\"/>\n");
___v1ew.push("        </li>\n");
___v1ew.push("        <li>\n");
___v1ew.push("          <label for=\"shipstate\">state</label>\n");
___v1ew.push("          <select id=\"shipstate\" name=\"shipstate\" disabled=\"disabled\" class=\"\">\n");
___v1ew.push("            <option value=\"\" selected=\"selected\">Select a State</option>\n");
___v1ew.push("            <option value=\"AL\">Alabama</option>\n");
___v1ew.push("            <option value=\"AK\">Alaska</option>\n");
___v1ew.push("            <option value=\"AZ\">Arizona</option>\n");
___v1ew.push("            <option value=\"AR\">Arkansas</option>\n");
___v1ew.push("            <option value=\"CA\">California</option>\n");
___v1ew.push("            <option value=\"CO\">Colorado</option>\n");
___v1ew.push("            <option value=\"CT\">Connecticut</option>\n");
___v1ew.push("            <option value=\"DE\">Delaware</option>\n");
___v1ew.push("            <option value=\"DC\">District Of Columbia</option>\n");
___v1ew.push("            <option value=\"FL\">Florida</option>\n");
___v1ew.push("            <option value=\"GA\">Georgia</option>\n");
___v1ew.push("            <option value=\"HI\">Hawaii</option>\n");
___v1ew.push("            <option value=\"ID\">Idaho</option>\n");
___v1ew.push("            <option value=\"IL\">Illinois</option>\n");
___v1ew.push("            <option value=\"IN\">Indiana</option>\n");
___v1ew.push("            <option value=\"IA\">Iowa</option>\n");
___v1ew.push("            <option value=\"KS\">Kansas</option>\n");
___v1ew.push("            <option value=\"KY\">Kentucky</option>\n");
___v1ew.push("            <option value=\"LA\">Louisiana</option>\n");
___v1ew.push("            <option value=\"ME\">Maine</option>\n");
___v1ew.push("            <option value=\"MD\">Maryland</option>\n");
___v1ew.push("            <option value=\"MA\">Massachusetts</option>\n");
___v1ew.push("            <option value=\"MI\">Michigan</option>\n");
___v1ew.push("            <option value=\"MN\">Minnesota</option>\n");
___v1ew.push("            <option value=\"MS\">Mississippi</option>\n");
___v1ew.push("            <option value=\"MO\">Missouri</option>\n");
___v1ew.push("            <option value=\"MT\">Montana</option>\n");
___v1ew.push("            <option value=\"NE\">Nebraska</option>\n");
___v1ew.push("            <option value=\"NV\">Nevada</option>\n");
___v1ew.push("            <option value=\"NH\">New Hampshire</option>\n");
___v1ew.push("            <option value=\"NJ\">New Jersey</option>\n");
___v1ew.push("            <option value=\"NM\">New Mexico</option>\n");
___v1ew.push("            <option value=\"NY\">New York</option>\n");
___v1ew.push("            <option value=\"NC\">North Carolina</option>\n");
___v1ew.push("            <option value=\"ND\">North Dakota</option>\n");
___v1ew.push("            <option value=\"OH\">Ohio</option>\n");
___v1ew.push("            <option value=\"OK\">Oklahoma</option>\n");
___v1ew.push("            <option value=\"OR\">Oregon</option>\n");
___v1ew.push("            <option value=\"PA\">Pennsylvania</option>\n");
___v1ew.push("            <option value=\"RI\">Rhode Island</option>\n");
___v1ew.push("            <option value=\"SC\">South Carolina</option>\n");
___v1ew.push("            <option value=\"SD\">South Dakota</option>\n");
___v1ew.push("            <option value=\"TN\">Tennessee</option>\n");
___v1ew.push("            <option value=\"TX\">Texas</option>\n");
___v1ew.push("            <option value=\"UT\">Utah</option>\n");
___v1ew.push("            <option value=\"VT\">Vermont</option>\n");
___v1ew.push("            <option value=\"VA\">Virginia</option>\n");
___v1ew.push("            <option value=\"WA\">Washington</option>\n");
___v1ew.push("            <option value=\"WV\">West Virginia</option>\n");
___v1ew.push("            <option value=\"WI\">Wisconsin</option>\n");
___v1ew.push("            <option value=\"WY\">Wyoming</option>\n");
___v1ew.push("          </select>\n");
___v1ew.push("        </li>\n");
___v1ew.push("        <li>\n");
___v1ew.push("          <label for=\"shipzip\">zip</label>\n");
___v1ew.push("          <input id=\"shipzip\" name=\"shipzip\" type=\"text\" disabled=\"disabled\" class=\"zip\" value=\"");___v1ew.push((jQuery.EJS.text(customer.shipzip)));___v1ew.push("\"/>\n");
___v1ew.push("        </li>\n");
___v1ew.push("      </ul>\n");
___v1ew.push("    </div>\n");
___v1ew.push("    <div id=\"customerErrorBox\">\n");
___v1ew.push("        <span></span>\n");
___v1ew.push("        <ul></ul>\n");
___v1ew.push("    </div>\n");
___v1ew.push("</form>");; return ___v1ew.join('');}}}catch(e){e.lineNumber=null;throw e;} }));;
steal.end();
$.View.preload('mgmtransformer_views_customer_show_ejs',jQuery.EJS(function(_CONTEXT,_VIEW) { try { with(_VIEW) { with (_CONTEXT) {var ___v1ew = [];___v1ew.push("﻿<div id=\"job\" class=\"show\">\n");
___v1ew.push("  <h4>Customer Info</h4>\n");
___v1ew.push("  <ul>\n");
___v1ew.push("    <li></li>\n");
___v1ew.push("    <li>\n");
___v1ew.push("      <span id=\"jobname\">");___v1ew.push((jQuery.EJS.text(customer.jobname)));___v1ew.push("</span>\n");
___v1ew.push("    </li>\n");
___v1ew.push("    <li>\n");
___v1ew.push("		<span id=\"contactname\">");___v1ew.push((jQuery.EJS.text(customer.contactname)));___v1ew.push("</span>\n");
___v1ew.push("    </li>\n");
___v1ew.push("    <li>\n");
___v1ew.push("		<span id=\"telephone\">");___v1ew.push((jQuery.EJS.text(customer.telephone)));___v1ew.push("</span>\n");
___v1ew.push("    </li>\n");
___v1ew.push("    <li>\n");
___v1ew.push("		<span id=\"email\">");___v1ew.push((jQuery.EJS.text(customer.email)));___v1ew.push("</span>\n");
___v1ew.push("    </li>\n");
___v1ew.push("  </ul>\n");
___v1ew.push("</div>\n");
___v1ew.push("\n");
___v1ew.push("<div id=\"billing\" class=\"show\">\n");
___v1ew.push("  <h4>Billing Address</h4>\n");
___v1ew.push("  <ul>\n");
___v1ew.push("    <li></li>\n");
___v1ew.push("    <li>\n");
___v1ew.push("		<span id=\"company\">");___v1ew.push((jQuery.EJS.text(customer.company)));___v1ew.push("</span>\n");
___v1ew.push("    </li>\n");
___v1ew.push("    <li>\n");
___v1ew.push("		<span id=\"addressline1\">");___v1ew.push((jQuery.EJS.text(customer.addressline1)));___v1ew.push("</span>\n");
___v1ew.push("    </li>\n");
___v1ew.push("    <li>\n");
___v1ew.push("		<span id=\"addressline2\">");___v1ew.push((jQuery.EJS.text(customer.addressline2)));___v1ew.push("</span>\n");
___v1ew.push("    </li>\n");
___v1ew.push("    <li>\n");
___v1ew.push("		<span id=\"city\">");___v1ew.push((jQuery.EJS.text(customer.city)));___v1ew.push("</span>, \n");
___v1ew.push("		<span id=\"state\">");___v1ew.push((jQuery.EJS.text(customer.state)));___v1ew.push("</span> \n");
___v1ew.push("		<span id=\"zip\">");___v1ew.push((jQuery.EJS.text(customer.zip)));___v1ew.push("</span>\n");
___v1ew.push("    </li>\n");
___v1ew.push("  </ul>\n");
___v1ew.push("</div>\n");
___v1ew.push("\n");
___v1ew.push("<div id=\"shipping\" class=\"show\">\n");
___v1ew.push("  <h4>Shipping To</h4>\n");
___v1ew.push("  <ul>\n");
___v1ew.push("    <li>\n");
___v1ew.push("		<span id=\"shipcompany\">");___v1ew.push((jQuery.EJS.text(customer.shipcompany)));___v1ew.push("</span>\n");
___v1ew.push("    </li>\n");
___v1ew.push("    <li>\n");
___v1ew.push("		<span id=\"shipaddressline1\">");___v1ew.push((jQuery.EJS.text(customer.shipaddressline1)));___v1ew.push("</span>\n");
___v1ew.push("    </li>\n");
___v1ew.push("    <li>\n");
___v1ew.push("		<span id=\"shipaddressline2\">");___v1ew.push((jQuery.EJS.text(customer.shipaddressline2)));___v1ew.push("</span>\n");
___v1ew.push("    </li>\n");
___v1ew.push("    <li>\n");
___v1ew.push("		<span id=\"shipcity\">");___v1ew.push((jQuery.EJS.text(customer.shipcity)));___v1ew.push("</span>, \n");
___v1ew.push("		<span id=\"shipstate\">");___v1ew.push((jQuery.EJS.text(customer.shipstate)));___v1ew.push("</span> \n");
___v1ew.push("		<span id=\"shipzip\">");___v1ew.push((jQuery.EJS.text(customer.shipzip)));___v1ew.push("</span>\n");
___v1ew.push("    </li>\n");
___v1ew.push("  </ul>\n");
___v1ew.push("</div>");; return ___v1ew.join('');}}}catch(e){e.lineNumber=null;throw e;} }));;
steal.end();
$.View.preload('mgmtransformer_views_loader_loader_ejs',jQuery.EJS(function(_CONTEXT,_VIEW) { try { with(_VIEW) { with (_CONTEXT) {var ___v1ew = [];___v1ew.push("﻿<div class=\"loader\">\n");
___v1ew.push("	<h2>Please wait...</h2>\n");
___v1ew.push("	<div></div>\n");
___v1ew.push("</div>");; return ___v1ew.join('');}}}catch(e){e.lineNumber=null;throw e;} }));;
steal.end()