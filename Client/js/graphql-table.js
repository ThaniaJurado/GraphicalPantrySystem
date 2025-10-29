// GraphQL Table Component for Alpine.js
console.log('🔄 Cargando graphql-table.js...');

// Registrar componente de inventario directamente
function inventoryTable() {
    console.log('🚀 Creando instancia de inventoryTable');
    return {
        items: [],
        loading: false,
        error: '',
        
        // Configuración del servidor GraphQL
        graphqlEndpoint: 'https://localhost:5001/graphql/',
        
        init() {
            console.log('🚀 Inicializando componente inventoryTable');
            this.fetchInventory();
        },

    // Consulta GraphQL para obtener el inventario
    async fetchInventory() {
        this.loading = true;
        this.error = '';
        
        const query = `
            query getInventory{
                inventories{
                    pageInfo{
                    hasNextPage
                    hasPreviousPage
                    startCursor
                    endCursor
                    }
                    nodes{
                    expirationDate
                        item{
                            name
                            quantity
                            measurementUnit{
                            name
                            }
                        }
                    }
                }
            }
        `;

        try {
            const response = await this.executeGraphQLQuery(query);
            console.log('GraphQL response:', response);
            
            if (response.errors) {
                throw new Error(response.errors[0].message);
            }
            
            // Extraer los items de la estructura correcta
            this.items = response.data.inventories?.nodes || [];
            console.log('Items procesados:', this.items);
            console.log(`📊 Se encontraron ${this.items.length} elementos en el inventario`);
        } catch (error) {
            console.error('Error fetching inventory:', error);
            this.error = `Error al cargar el inventario: ${error.message}`;
        } finally {
            this.loading = false;
        }
    },

    // Función para ejecutar consultas GraphQL
    async executeGraphQLQuery(query, variables = {}) {
        const response = await fetch(this.graphqlEndpoint, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Accept': 'application/json',
            },
            body: JSON.stringify({
                query: query,
                variables: variables
            })
        });

        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }

        return await response.json();
    },

    // Función para formatear fechas
    formatDate(dateString) {
        if (!dateString) return 'N/A';
        
        const date = new Date(dateString);
        return date.toLocaleDateString('es-ES', {
            year: 'numeric',
            month: 'short',
            day: 'numeric',
            hour: '2-digit',
            minute: '2-digit'
        });
    },

    // Función para editar un elemento
    async editItem(item) {
        console.log('Editando item:', item);
        
        const newQuantity = prompt(`Nueva cantidad para ${item.item.name}:`, item.item.quantity);
        
        if (newQuantity !== null && !isNaN(newQuantity)) {
            await this.updateItemQuantity(item.id, parseInt(newQuantity));
        }
    },

    // Mutación para actualizar cantidad
    async updateItemQuantity(id, quantity) {
        this.loading = true;
        
        const mutation = `
            mutation UpdateItemQuantity($id: ID!, $quantity: Int!) {
                updateInventoryItem(id: $id, quantity: $quantity) {
                    id
                    name
                    quantity
                    category
                    updatedAt
                }
            }
        `;

        try {
            const response = await this.executeGraphQLQuery(mutation, { id, quantity });
            
            if (response.errors) {
                throw new Error(response.errors[0].message);
            }

            const updatedItem = response.data.updateInventoryItem;
            const index = this.items.findIndex(item => item.id === id);
            if (index !== -1) {
                this.items[index] = updatedItem;
            }
            
            console.log('Item actualizado exitosamente');
        } catch (error) {
            console.error('Error updating item:', error);
            this.error = `Error al actualizar el item: ${error.message}`;
        } finally {
            this.loading = false;
        }
    },

    // Función para eliminar un elemento
    async deleteItem(id) {
        if (!confirm('¿Estás seguro de que quieres eliminar este elemento?')) {
            return;
        }

        this.loading = true;
        
        const mutation = `
            mutation DeleteItem($id: ID!) {
                deleteInventoryItem(id: $id) {
                    success
                    message
                }
            }
        `;

        try {
            const response = await this.executeGraphQLQuery(mutation, { id });
            
            if (response.errors) {
                throw new Error(response.errors[0].message);
            }

            this.items = this.items.filter(item => item.id !== id);
            
            console.log('Item eliminado exitosamente');
        } catch (error) {
            console.error('Error deleting item:', error);
            this.error = `Error al eliminar el item: ${error.message}`;
        } finally {
            this.loading = false;
        }
    }
    };
}

// Hacer la función disponible globalmente
window.inventoryTable = inventoryTable;
console.log('📋 Función inventoryTable registrada globalmente');