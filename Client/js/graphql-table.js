// GraphQL Table Component for Alpine.js
console.log('🔄 Loading graphql-table.js...');

// Register inventory component directly
function inventoryTable() {
    console.log('🚀 Creating inventoryTable instance');
    return {
        items: [],
        loading: false,
        error: '',
        
        // Properties for new item form
        showNewItemForm: false,
        registeringItem: false,
        measurementUnits: [],
        newItem: {
            name: '',
            description: '',
            quantity: 0,
            measurementUnitId: ''
        },
        
        // Properties for new measurement unit form
        showNewUnitForm: false,
        registeringUnit: false,
        newUnit: {
            name: '',
            description: ''
        },
        
        // GraphQL server configuration
        graphqlEndpoint: 'https://localhost:5001/graphql/',
        
        init() {
            console.log('🚀 Initializing inventoryTable component');
            this.fetchInventory();
            this.fetchMeasurementUnits();
        },

    // GraphQL query to get inventory
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
                    id
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
            
            // Extract items from the correct structure
            this.items = response.data.inventories?.nodes || [];
            console.log('Processed items:', this.items);
            console.log(`📊 Found ${this.items.length} elements in inventory`);
        } catch (error) {
            console.error('Error fetching inventory:', error);
            this.error = `Error loading inventory: ${error.message}`;
        } finally {
            this.loading = false;
        }
    },

    // GraphQL query to get measurement units
    async fetchMeasurementUnits() {
        const query = `
          query GetMeasurementUnits{
                measurementUnits{
                    id
                    name
                    description
                }
            }
        `;

        try {
            const response = await this.executeGraphQLQuery(query);
            console.log('Measurement Units response:', response);
            
            if (response.errors) {
                throw new Error(response.errors[0].message);
            }
            
            this.measurementUnits = response.data.measurementUnits || [];
            console.log('📏 Measurement units loaded:', this.measurementUnits);
        } catch (error) {
            console.error('Error fetching measurement units:', error);
            this.error = `Error loading measurement units: ${error.message}`;
        }
    },

    // Show/hide new item form
    toggleNewItemForm() {
        this.showNewItemForm = !this.showNewItemForm;
        if (this.showNewItemForm) {
            this.resetNewItemForm();
        }
    },

    // Cancel new item
    cancelNewItem() {
        this.showNewItemForm = false;
        this.resetNewItemForm();
    },

    // Reset new item form
    resetNewItemForm() {
        this.newItem = {
            name: '',
            description: '',
            quantity: 0,
            measurementUnitId: ''
        };
        this.error = '';
    },

    // Register new item
    async registerNewItem() {
        console.log("measurementunitis "+this.newItem.measurementUnitId);
        if (!this.newItem.name || !this.newItem.measurementUnitId) {
            this.error = 'Please complete all required fields';
            return;
        }

        this.registeringItem = true;
        this.error = '';

        const mutation = `
mutation AddToItems($inputItem: ItemInputTypeInput!) {
                addToItems(inputItem:  $inputItem) {
                 id
                }
            }
        `;

        const variables = {
            inputItem: {
                name: this.newItem.name,
                description: this.newItem.description || null,
                quantity: parseFloat(this.newItem.quantity),
                measurementUnitId: parseInt(this.newItem.measurementUnitId)
            }
        };

        try {
            const response = await this.executeGraphQLQuery(mutation, variables);
            console.log('Create item response:', response);
            
            if (response.errors) {
                throw new Error(response.errors[0].message);
            }

            console.log('✅ Item created successfully:', response.data.id);
            
            // Close form and reset
            this.showNewItemForm = false;
            this.resetNewItemForm();
            
            // Optional: update inventory list
            // this.fetchInventory();
            
            // Show success message (optional)
            alert('Item registered successfully!');
            
        } catch (error) {
            console.error('Error registering item:', error);
            this.error = `Error registering item: ${error.message}`;
        } finally {
            this.registeringItem = false;
        }
    },

    // Show/hide new measurement unit form
    toggleNewUnitForm() {
        this.showNewUnitForm = !this.showNewUnitForm;
        if (this.showNewUnitForm) {
            this.resetNewUnitForm();
        }
    },

    // Cancel new measurement unit
    cancelNewUnit() {
        this.showNewUnitForm = false;
        this.resetNewUnitForm();
    },

    // Reset new measurement unit form
    resetNewUnitForm() {
        this.newUnit = {
            name: '',
            description: ''
        };
        this.error = '';
    },

    // Register new measurement unit
    async registerNewUnit() {
        if (!this.newUnit.name) {
            this.error = 'Please enter the measurement unit name';
            return;
        }

        this.registeringUnit = true;
        this.error = '';

        const mutation = `
            mutation AddToMeasurementUnits($inputMeasurementUnit: MeasurementUnitInputTypeInput!) {
                addToMeasurementUnits(inputMeasurementUnit:  $inputMeasurementUnit) {
                 id
                }
            }
        `;

        const variables = {
            inputMeasurementUnit: {
                name: this.newUnit.name,
                description: this.newUnit.description || null
            }
        };

        try {
            const response = await this.executeGraphQLQuery(mutation, variables);
            console.log('Create measurement unit response:', response);
            
            if (response.errors) {
                throw new Error(response.errors[0].message);
            }

            console.log('✅ Measurement unit created successfully:', response.data.id);
            
            // Close form and reset
            this.showNewUnitForm = false;
            this.resetNewUnitForm();
            
            // Update measurement units list
            await this.fetchMeasurementUnits();
            
            // Show success message
            alert('Measurement unit added successfully!');
            
        } catch (error) {
            console.error('Error registering measurement unit:', error);
            this.error = `Error adding measurement unit: ${error.message}`;
        } finally {
            this.registeringUnit = false;
        }
    },

    // Function to execute GraphQL queries
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
        return date.toLocaleDateString('en-US', {
            year: 'numeric',
            month: 'short',
            day: 'numeric',
            hour: '2-digit',
            minute: '2-digit'
        });
    },

    // Function to edit an item
    async editItem(item) {
        console.log('Editing item:', item);
        
        const newQuantity = prompt(`New quantity for ${item.item.name}:`, item.item.quantity);
        
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
            
            console.log('Item updated successfully');
        } catch (error) {
            console.error('Error updating item:', error);
            this.error = `Error updating item: ${error.message}`;
        } finally {
            this.loading = false;
        }
    },

    // Function to delete an item
    async deleteItem(id) {
        if (!confirm('Are you sure you want to delete this item?')) {
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
            
            console.log('Item deleted successfully');
        } catch (error) {
            console.error('Error deleting item:', error);
            this.error = `Error deleting item: ${error.message}`;
        } finally {
            this.loading = false;
        }
    }
    };
}

// Make function globally available
window.inventoryTable = inventoryTable;
console.log('📋 inventoryTable function registered globally');