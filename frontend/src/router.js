import { createRouter, createWebHistory } from 'vue-router'
import Login from './views/Login.vue'
import UserTickets from './views/UserTickets.vue'
import AdminTickets from './views/AdminTickets.vue'

const routes = [
  { path: '/', component: Login },
  { path: '/user-tickets', component: UserTickets },
  { path: '/admin-tickets', component: AdminTickets }
]

const router = createRouter({
  history: createWebHistory(),
  routes
})

export default router