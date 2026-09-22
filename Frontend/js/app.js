const API_BASE = 'http://localhost:5233/api';
const vehicleFallback = [
  { vehicleId: 1, vehicleNumber: 'TS09AB1234', ownerName: 'Manoj Kumar', vehicleType: 'Car', createdAt: '05 Sep 2026' },
  { vehicleId: 2, vehicleNumber: 'KA01MN7788', ownerName: 'Kavya Rao', vehicleType: 'SUV', createdAt: '04 Sep 2026' },
  { vehicleId: 3, vehicleNumber: 'AP28CQ0912', ownerName: 'R. Prasad', vehicleType: 'Truck', createdAt: '03 Sep 2026' },
  { vehicleId: 4, vehicleNumber: 'TS07EF4401', ownerName: 'Nisha Menon', vehicleType: 'Car', createdAt: '02 Sep 2026' },
  { vehicleId: 5, vehicleNumber: 'MH12LK8820', ownerName: 'Arun Varma', vehicleType: 'Bus', createdAt: '01 Sep 2026' },
  { vehicleId: 6, vehicleNumber: 'TS10GH6009', ownerName: 'S. Ramesh', vehicleType: 'Car', createdAt: '31 Aug 2026' }
];

const state = { vehicles: [...vehicleFallback], apiConnected: false };
const $ = (selector, parent = document) => parent.querySelector(selector);
const $$ = (selector, parent = document) => [...parent.querySelectorAll(selector)];

function showToast(message, isError = false) {
  const toast = $('#toast');
  $('#toastMessage').textContent = message;
  toast.style.background = isError ? '#9b4842' : '';
  toast.classList.add('show');
  window.clearTimeout(showToast.timer);
  showToast.timer = window.setTimeout(() => toast.classList.remove('show'), 3000);
}

function setConnection(connected) {
  state.apiConnected = connected;
  const status = $('#connectionStatus');
  status.classList.toggle('offline', !connected);
  $('.status-dot', status).classList.toggle('offline', !connected);
  $('span:last-child', status).textContent = connected ? 'API connected' : 'Demo mode';
}

function navigate(viewName) {
  $$('.view').forEach(view => view.classList.toggle('active', view.id === `view-${viewName}`));
  $$('.nav-item').forEach(item => item.classList.toggle('active', item.dataset.view === viewName));
  const active = $(`#view-${viewName}`);
  $('#breadcrumbCurrent').textContent = active?.querySelector('h1')?.textContent || 'Dashboard';
  $('#sidebar').classList.remove('open');
  if (viewName === 'vehicles') renderVehicles();
  window.scrollTo({ top: 0, behavior: 'smooth' });
}

function renderVehicles(filter = '') {
  const query = filter.toLowerCase().trim();
  const rows = state.vehicles.filter(vehicle => [vehicle.vehicleNumber, vehicle.ownerName, vehicle.vehicleType].join(' ').toLowerCase().includes(query));
  $('#vehicleCount').textContent = rows.length;
  $('#vehicleRows').innerHTML = rows.length ? rows.map(vehicle => `
    <tr>
      <td><strong>${escapeHtml(vehicle.vehicleNumber)}</strong><small>Vehicle ID #${vehicle.vehicleId}</small></td>
      <td>${escapeHtml(vehicle.ownerName)}</td>
      <td><span class="type-chip">${escapeHtml(vehicle.vehicleType)}</span></td>
      <td>${escapeHtml(vehicle.createdAt || 'Today')}</td>
      <td><span class="vehicle-status"><i></i>Active</span></td>
      <td><button class="row-actions" data-delete-id="${vehicle.vehicleId}" aria-label="Delete vehicle">•••</button></td>
    </tr>`).join('') : '<tr><td colspan="6" class="empty-state">No vehicles match this search.</td></tr>';
}

function escapeHtml(value) {
  return String(value ?? '').replace(/[&<>'"]/g, character => ({ '&': '&amp;', '<': '&lt;', '>': '&gt;', "'": '&#39;', '"': '&quot;' }[character]));
}

async function loadVehicles() {
  try {
    const response = await fetch(`${API_BASE}/vehicle`, { headers: { Accept: 'application/json' } });
    if (!response.ok) throw new Error('Vehicle endpoint unavailable');
    const data = await response.json();
    state.vehicles = Array.isArray(data) ? data : [];
    setConnection(true);
    $('#vehicleSync').textContent = 'Synced with API just now';
  } catch (error) {
    setConnection(false);
    $('#vehicleSync').textContent = 'Showing demo data · API unavailable';
  }
  renderVehicles($('#vehicleSearch').value);
}

function openVehicleModal() {
  $('#vehicleModal').classList.add('open');
  $('#vehicleModal').setAttribute('aria-hidden', 'false');
  $('[name="vehicleNumber"]').focus();
}

function closeVehicleModal() {
  $('#vehicleModal').classList.remove('open');
  $('#vehicleModal').setAttribute('aria-hidden', 'true');
  $('#vehicleForm').reset();
}

async function submitVehicle(event) {
  event.preventDefault();
  const form = new FormData(event.currentTarget);
  const vehicle = Object.fromEntries(form.entries());
  const saveButton = $('#saveVehicle');
  saveButton.disabled = true;
  saveButton.textContent = 'Saving...';
  try {
    const response = await fetch(`${API_BASE}/vehicle`, { method: 'POST', headers: { 'Content-Type': 'application/json', Accept: 'application/json' }, body: JSON.stringify(vehicle) });
    if (!response.ok) throw new Error('Unable to save vehicle');
    const created = await response.json();
    state.vehicles.unshift({ ...vehicle, ...created, createdAt: 'Today' });
    setConnection(true);
    $('#vehicleSync').textContent = 'Synced with API just now';
    showToast('Vehicle added successfully');
  } catch (error) {
    state.vehicles.unshift({ ...vehicle, vehicleId: `D-${Date.now()}`, createdAt: 'Today' });
    showToast('Saved locally in demo mode');
  } finally {
    saveButton.disabled = false;
    saveButton.textContent = 'Save vehicle';
    closeVehicleModal();
    renderVehicles($('#vehicleSearch').value);
  }
}

async function deleteVehicle(id) {
  const vehicle = state.vehicles.find(item => String(item.vehicleId) === String(id));
  if (!vehicle || !window.confirm(`Remove ${vehicle.vehicleNumber} from the registry?`)) return;
  try {
    const response = await fetch(`${API_BASE}/vehicle/${id}`, { method: 'DELETE' });
    if (!response.ok) throw new Error('Delete failed');
    showToast('Vehicle removed');
  } catch (error) {
    showToast('Removed from demo view');
  }
  state.vehicles = state.vehicles.filter(item => String(item.vehicleId) !== String(id));
  renderVehicles($('#vehicleSearch').value);
}

function init() {
  $$('.nav-item').forEach(item => item.addEventListener('click', () => navigate(item.dataset.view)));
  $$('[data-view-target]').forEach(item => item.addEventListener('click', () => navigate(item.dataset.viewTarget)));
  $('#menuToggle').addEventListener('click', () => $('#sidebar').classList.toggle('open'));
  $('#openVehicleModal').addEventListener('click', openVehicleModal);
  $('#closeVehicleModal').addEventListener('click', closeVehicleModal);
  $('#cancelVehicle').addEventListener('click', closeVehicleModal);
  $('#vehicleModal').addEventListener('click', event => { if (event.target.id === 'vehicleModal') closeVehicleModal(); });
  $('#vehicleForm').addEventListener('submit', submitVehicle);
  $('#vehicleSearch').addEventListener('input', event => renderVehicles(event.target.value));
  $('#vehicleRows').addEventListener('click', event => { const button = event.target.closest('[data-delete-id]'); if (button) deleteVehicle(button.dataset.deleteId); });
  $('#refreshDashboard').addEventListener('click', event => { const button = event.currentTarget; button.classList.add('spinning'); window.setTimeout(() => button.classList.remove('spinning'), 500); showToast('Dashboard refreshed'); loadVehicles(); });
  $$('.toggle').forEach(toggle => toggle.addEventListener('click', () => toggle.classList.toggle('on')));
  document.addEventListener('keydown', event => { if (event.key === 'Escape') closeVehicleModal(); });
  renderVehicles();
  loadVehicles();
}

document.addEventListener('DOMContentLoaded', init);
