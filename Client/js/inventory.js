document.addEventListener('Alpine:init', ()=>{
    Alpine.data('inventory', ()=>({
       loading: false,
       edition: false,
       id: 0,
       form: {
           name: '',
           quantity: '',
       }
    }));
});
