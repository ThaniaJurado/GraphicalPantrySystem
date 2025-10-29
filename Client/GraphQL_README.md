# Ejemplo de servidor GraphQL para el Sistema de Inventario

Este archivo contiene ejemplos de las consultas y mutaciones GraphQL que tu servidor debe implementar.

## Schema de GraphQL

```graphql
# Tipo de datos para los elementos del inventario
type InventoryItem {
  id: ID!
  name: String!
  quantity: Int!
  category: String!
  updatedAt: String!
}

# Tipo de respuesta para operaciones de eliminación
type DeleteResponse {
  success: Boolean!
  message: String!
}

# Consultas (Queries)
type Query {
  # Obtener todos los elementos del inventario
  inventory: [InventoryItem!]!
  
  # Obtener un elemento específico por ID
  inventoryItem(id: ID!): InventoryItem
}

# Mutaciones (Mutations)
type Mutation {
  # Actualizar la cantidad de un elemento
  updateInventoryItem(id: ID!, quantity: Int!): InventoryItem!
  
  # Eliminar un elemento del inventario
  deleteInventoryItem(id: ID!): DeleteResponse!
  
  # Crear un nuevo elemento (opcional)
  createInventoryItem(name: String!, quantity: Int!, category: String!): InventoryItem!
}
```

## Ejemplos de consultas GraphQL

### 1. Obtener todos los elementos del inventario
```graphql
query GetInventory {
  inventory {
    id
    name
    quantity
    category
    updatedAt
  }
}
```

### 2. Obtener un elemento específico
```graphql
query GetInventoryItem($id: ID!) {
  inventoryItem(id: $id) {
    id
    name
    quantity
    category
    updatedAt
  }
}
```

### 3. Actualizar cantidad de un elemento
```graphql
mutation UpdateItemQuantity($id: ID!, $quantity: Int!) {
  updateInventoryItem(id: $id, quantity: $quantity) {
    id
    name
    quantity
    category
    updatedAt
  }
}
```

### 4. Eliminar un elemento
```graphql
mutation DeleteItem($id: ID!) {
  deleteInventoryItem(id: $id) {
    success
    message
  }
}
```

### 5. Crear un nuevo elemento
```graphql
mutation CreateItem($name: String!, $quantity: Int!, $category: String!) {
  createInventoryItem(name: $name, quantity: $quantity, category: $category) {
    id
    name
    quantity
    category
    updatedAt
  }
}
```

## Configuración del cliente

Para usar este código con tu servidor GraphQL:

1. **Cambia la URL del endpoint** en `js/graphql-table.js`:
   ```javascript
   graphqlEndpoint: 'http://tu-servidor.com/graphql'
   ```

2. **Agrega autenticación** si es necesaria:
   ```javascript
   headers: {
     'Content-Type': 'application/json',
     'Accept': 'application/json',
     'Authorization': 'Bearer tu-token-aqui'
   }
   ```

3. **Ajusta el esquema** según tus necesidades específicas.

## Datos de ejemplo

Aquí tienes algunos datos de ejemplo que tu servidor podría devolver:

```json
{
  "data": {
    "inventory": [
      {
        "id": "1",
        "name": "Arroz",
        "quantity": 45,
        "category": "Granos",
        "updatedAt": "2025-10-29T10:30:00Z"
      },
      {
        "id": "2", 
        "name": "Leche",
        "quantity": 8,
        "category": "Lácteos",
        "updatedAt": "2025-10-29T09:15:00Z"
      },
      {
        "id": "3",
        "name": "Pan",
        "quantity": 120,
        "category": "Panadería",
        "updatedAt": "2025-10-29T11:45:00Z"
      }
    ]
  }
}
```