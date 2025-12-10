<template>
  <div class="admin-page">
    <div class="tickets-card">
      <div class="tickets-header">
        <h1>Admin - alla ärenden</h1>
        <button type="button" class="secondary-btn" @click="loadTickets">
          Uppdatera lista
        </button>
      </div>
      <ul v-if="tickets.length" class="ticket-list">
        <li v-for="ticket in tickets" :key="ticket.id" class="ticket-item">
          <div class="ticket-header">
            <div>
              <div class="ticket-title">{{ ticket.title }}</div>
              <div class="ticket-description">{{ ticket.description }}</div>
            </div>
            <span class="ticket-status">{{ ticket.statusName }}</span>
          </div>
          <div class="ticket-actions">
            <select v-model.number="ticket.statusId" class="input">
              <option v-for="s in statuses" :key="s.id" :value="s.id">
                {{ s.name }}
              </option>
            </select>
            <button @click="updateStatus(ticket)" class="primary-btn">
              uppdatera status
            </button>
            <button @click="deleteTicket(ticket)" class="danger-btn">
              ta bort
            </button>
          </div>
        </li>
      </ul>
      <p v-else class="empty">Inga ärenden ännu.</p>
      <p v-if="error" class="error">{{ error }}</p>
    </div>
  </div>
</template>

<script>
import axios from 'axios';

export default {
  name: 'AdminTickets',
  data() {
    return {
      tickets: [],
      statuses: [
        { id: 1, name: 'Öppen' },
        { id: 2, name: 'Pågående' },
        { id: 3, name: 'Löst' },
        { id: 4, name: 'Stängd' },
      ],
      error: '',
    };
  },
  created() {
    this.loadTickets();
  },
  methods: {
    async loadTickets() {
      this.error = '';
      try {
        const response = await axios.get('http://localhost:5000/api/tickets');
        this.tickets = response.data.map(t => ({
          id: t.id,
          title: t.title,
          description: t.description,
          statusId: t.statusId,
          statusName: this.getStatusName(t.statusId),
        }));
      } catch (err) {
        this.error = 'Kunde inte hämta ärenden.';
        console.error(err);
      }
    },
    getStatusName(id) {
      const s = this.statuses.find(x => x.id === id);
      return s ? s.name : '';
    },
    async updateStatus(ticket) {
      this.error = '';
      try {
        await axios.put(
          `http://localhost:5000/api/tickets/${ticket.id}/status`,
          { statusId: ticket.statusId } // skicka som objekt
        );
        ticket.statusName = this.getStatusName(ticket.statusId);
      } catch (err) {
        this.error = 'Kunde inte uppdatera status.';
        console.error(err);
      }
    },
    async deleteTicket(ticket) {
      this.error = '';
      if (!confirm(`Ta bort ärende "${ticket.title}"?`)) {
        return;
      }
      try {
        await axios.delete(`http://localhost:5000/api/tickets/${ticket.id}`);
        this.tickets = this.tickets.filter(t => t.id !== ticket.id);
      } catch (err) {
        this.error = 'Kunde inte ta bort ärendet.';
        console.error(err);
      }
    },
  },
};
</script>

<style scoped>
.admin-page {
  max-width: 900px;
  margin: 40px auto;
  font-family: system-ui, -apple-system, BlinkMacSystemFont, 'Segoe UI', sans-serif;
}

.tickets-card {
  background: #ffffff;
  border-radius: 10px;
  padding: 18px 18px 16px;
  box-shadow: 0 8px 20px rgba(15, 23, 42, 0.08);
}

.tickets-card h1 {
  margin: 0;
}

.ticket-list {
  list-style: none;
  margin: 0;
  padding: 0;
  display: flex;
  flex-direction: column;
  gap: 10px;
}

.tickets-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 14px;
}

.secondary-btn {
  padding: 6px 10px;
  border-radius: 6px;
  border: 1px solid #d0d0d0;
  background: #f3f4f6;
  color: #374151;
  cursor: pointer;
  font-size: 0.85rem;
}

.secondary-btn:hover {
  background: #e5e7eb;
}

.ticket-item {
  padding: 10px 11px;
  border-radius: 8px;
  border: 1px solid #e5e7eb;
  background: #f9fafb;
}

.ticket-header {
  display: flex;
  justify-content: space-between;
  align-items: flex-start;
  margin-bottom: 6px;
}

.ticket-title {
  font-weight: 600;
}

.ticket-description {
  font-size: 0.9rem;
  color: #4b5563;
}

.ticket-status {
  font-size: 0.85rem;
  padding: 2px 8px;
  border-radius: 999px;
  background: #e0f2fe;
  color: #0369a1;
}

.ticket-actions {
  display: flex;
  gap: 8px;
  align-items: center;
}

.input {
  padding: 6px 8px;
  border-radius: 6px;
  border: 1px solid #d0d0d0;
  font-size: 0.9rem;
}

.input:focus {
  outline: none;
  border-color: #2563eb;
  box-shadow: 0 0 0 1px #2563eb33;
}

.primary-btn {
  padding: 6px 10px;
  border-radius: 6px;
  border: none;
  background: #2563eb;
  color: #fff;
  cursor: pointer;
  font-size: 0.9rem;
}

.primary-btn:hover {
  background: #1d4ed8;
}

.danger-btn {
  padding: 6px 10px;
  border-radius: 6px;
  border: none;
  background: #dc2626;
  color: #fff;
  cursor: pointer;
  font-size: 0.9rem;
}

.danger-btn:hover {
  background: #b91c1c;
}

.error {
  margin-top: 10px;
  color: #b91c1c;
}

.empty {
  font-size: 0.9rem;
  color: #6b7280;
}
</style>