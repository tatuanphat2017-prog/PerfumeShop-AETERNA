/* ===========================================================
   AETERNA — Tiện ích dùng chung cho toàn bộ frontend
   =========================================================== */

// Frontend được ASP.NET phục vụ cùng nguồn, nên API ở /api
const API_BASE = '/api';

/* ---------- Lưu trữ phiên đăng nhập ---------- */
const Auth = {
  get token() { return localStorage.getItem('aeterna_token'); },
  get user() {
    const raw = localStorage.getItem('aeterna_user');
    return raw ? JSON.parse(raw) : null;
  },
  get isLoggedIn() { return !!this.token; },
  get isAdmin() { return this.user?.role === 'Admin'; },
  save(data) {
    localStorage.setItem('aeterna_token', data.token);
    localStorage.setItem('aeterna_user', JSON.stringify({
      fullName: data.fullName, email: data.email, role: data.role
    }));
  },
  logout() {
    localStorage.removeItem('aeterna_token');
    localStorage.removeItem('aeterna_user');
    location.href = 'index.html';
  }
};

/* ---------- Gọi API ----------
   Trả về dữ liệu JSON, ném lỗi kèm message từ server nếu có. */
async function api(path, { method = 'GET', body = null, auth = false } = {}) {
  const headers = { 'Content-Type': 'application/json' };
  if (auth && Auth.token) headers['Authorization'] = `Bearer ${Auth.token}`;

  const res = await fetch(`${API_BASE}${path}`, {
    method,
    headers,
    body: body ? JSON.stringify(body) : null
  });

  if (res.status === 401) {
    // Token hết hạn hoặc chưa đăng nhập
    if (auth) { Auth.logout(); }
    throw new Error('Bạn cần đăng nhập để tiếp tục.');
  }

  const text = await res.text();
  const data = text ? JSON.parse(text) : null;

  if (!res.ok) {
    throw new Error(data?.message || 'Đã có lỗi xảy ra, vui lòng thử lại.');
  }
  return data;
}

/* ---------- Định dạng tiền VND ---------- */
function formatVND(amount) {
  return new Intl.NumberFormat('vi-VN', {
    style: 'currency', currency: 'VND', maximumFractionDigits: 0
  }).format(amount);
}

/* ---------- Thông báo nhỏ (toast) ---------- */
function toast(message) {
  let el = document.querySelector('.toast');
  if (!el) {
    el = document.createElement('div');
    el.className = 'toast';
    document.body.appendChild(el);
  }
  el.textContent = message;
  el.classList.add('show');
  clearTimeout(el._t);
  el._t = setTimeout(() => el.classList.remove('show'), 2600);
}

/* ---------- Ảnh sản phẩm (dự phòng khi trống hoặc lỗi tải) ---------- */
function productImage(p) {
  if (p.imageUrl) {
    const label = (p.name || 'AETERNA').split(' ')[0].replace(/["'<>]/g, '');
    return `<img src="${p.imageUrl}" alt="${p.name}" onerror="imgFallback(this, '${label}')">`;
  }
  return placeholderHtml(p.name);
}
function placeholderHtml(name) {
  const label = (name || 'AETERNA').split(' ')[0];
  return `<div class="product-placeholder">${label}</div>`;
}
// Khi ảnh lỗi, thay bằng ô placeholder (tránh vỡ layout)
function imgFallback(img, label) {
  img.parentNode.innerHTML = `<div class="product-placeholder">${label}</div>`;
}

/* ---------- Nhúng font icon Material Symbols (1 lần) ---------- */
function ensureIconFont() {
  if (document.getElementById('material-symbols')) return;
  const link = document.createElement('link');
  link.id = 'material-symbols';
  link.rel = 'stylesheet';
  link.href = 'https://fonts.googleapis.com/css2?family=Material+Symbols+Outlined:opsz,wght,FILL,GRAD@24,400,0,0';
  document.head.appendChild(link);
}

/* ---------- Render Header ---------- */
function renderHeader(active = '') {
  ensureIconFont();
  const links = [
    { href: 'index.html', label: 'Trang chủ', key: 'home' },
    { href: 'shop.html', label: 'Sản phẩm', key: 'shop' },
    { href: 'about.html', label: 'Giới thiệu', key: 'about' },
    { href: 'contact.html', label: 'Liên hệ', key: 'contact' }
  ];
  if (Auth.isAdmin) links.push({ href: 'admin.html', label: 'Quản trị', key: 'admin' });

  const accountArea = Auth.isLoggedIn
    ? `<button class="icon-btn" onclick="Auth.logout()" title="Đăng xuất">
         <span class="material-symbols-outlined">logout</span>
         <span class="hide-sm">${Auth.user.fullName.split(' ').slice(-1)[0]}</span>
       </button>`
    : `<a class="icon-btn" href="account.html" title="Đăng nhập">
         <span class="material-symbols-outlined">person</span>
       </a>`;

  document.getElementById('header').innerHTML = `
    <header class="site-header">
      <div class="container nav">
        <a class="logo" href="index.html">AETERNA</a>
        <ul class="nav-links" id="navLinks">
          ${links.map(l => `<li><a href="${l.href}" class="${active === l.key ? 'active' : ''}">${l.label}</a></li>`).join('')}
        </ul>
        <div class="nav-actions">
          <a class="icon-btn" href="shop.html" title="Tìm sản phẩm"><span class="material-symbols-outlined">search</span></a>
          ${accountArea}
          <a class="icon-btn" href="cart.html" title="Giỏ hàng">
            <span class="material-symbols-outlined">shopping_bag</span>
            <span class="cart-count hidden" id="cartCount">0</span>
          </a>
          <button class="icon-btn hamburger" onclick="document.getElementById('navLinks').classList.toggle('open')">
            <span class="material-symbols-outlined">menu</span>
          </button>
        </div>
      </div>
    </header>`;

  updateCartCount();
}

/* ---------- Render Footer ---------- */
function renderFooter() {
  document.getElementById('footer').innerHTML = `
    <footer class="site-footer">
      <div class="container">
        <div class="logo">AETERNA</div>
        <ul class="footer-links">
          <li><a href="about.html">Giới thiệu</a></li>
          <li><a href="shop.html">Sản phẩm</a></li>
          <li><a href="contact.html">Liên hệ</a></li>
          <li><a href="cart.html">Giỏ hàng</a></li>
        </ul>
        <div class="footer-social">
          <span class="material-symbols-outlined" title="Email">mail</span>
          <span class="material-symbols-outlined" title="Vị trí">location_on</span>
          <span class="material-symbols-outlined" title="Website">public</span>
        </div>
        <p class="footer-copy">© 2024 AETERNA FRAGRANCES · Đồ án cá nhân</p>
      </div>
    </footer>`;
}

/* ---------- Tạo các nhãn nốt hương (OUD, BERGAMOT…) ---------- */
function noteTags(notes) {
  if (!notes) return '';
  const tags = notes.split(',').map(s => s.trim()).filter(Boolean).slice(0, 3);
  return `<div class="note-tags">${tags.map(t => `<span class="note-tag">${t}</span>`).join('')}</div>`;
}

/* ---------- Tạo thẻ (card) sản phẩm dùng chung ---------- */
function renderProductCard(p) {
  const out = p.stock <= 0;
  return `
    <div class="product-card reveal${out ? ' out-of-stock' : ''}">
      <div class="product-media">
        ${productImage(p)}
        ${p.notes ? `<div class="media-tags">${noteTags(p.notes)}</div>` : ''}
        ${out ? '<span class="product-badge">Hết hàng</span>' : ''}
        <button class="quick-view-btn" onclick="openQuickView(${p.id})" title="Xem nhanh">
          <span class="material-symbols-outlined">visibility</span>
        </button>
        <button class="add-cart-btn" onclick="quickAddToCart(${p.id})" ${out ? 'disabled' : ''}>
          ${out ? 'Hết hàng' : 'Thêm vào giỏ'}
        </button>
      </div>
      <a href="product.html?id=${p.id}" class="product-info">
        <div>
          <h3>${p.name}</h3>
          <div class="notes">${p.brand || ''}</div>
        </div>
        <div class="product-price">${formatVND(p.price)}</div>
      </a>
    </div>`;
}

/* ---------- Thêm nhanh vào giỏ (yêu cầu đăng nhập) ---------- */
async function quickAddToCart(productId) {
  if (!Auth.isLoggedIn) {
    toast('Vui lòng đăng nhập để mua hàng.');
    setTimeout(() => location.href = 'account.html', 900);
    return;
  }
  try {
    await api('/cart/items', { method: 'POST', auth: true, body: { productId, quantity: 1 } });
    toast('Đã thêm vào giỏ hàng ✓');
    updateCartCount();
  } catch (e) { toast(e.message); }
}

/* ===========================================================
   Quick View — panel trượt từ phải khi bấm "xem nhanh"
   =========================================================== */
let qvQty = 1;

function ensureQuickView() {
  if (document.getElementById('qvDrawer')) return;
  ensureIconFont();
  const overlay = document.createElement('div');
  overlay.className = 'qv-overlay';
  overlay.id = 'qvOverlay';
  overlay.onclick = closeQuickView;

  const drawer = document.createElement('aside');
  drawer.className = 'qv-drawer';
  drawer.id = 'qvDrawer';
  drawer.innerHTML = `
    <button class="qv-close" onclick="closeQuickView()" title="Đóng">
      <span class="material-symbols-outlined">close</span>
    </button>
    <div class="qv-body" id="qvBody"></div>`;

  document.body.appendChild(overlay);
  document.body.appendChild(drawer);
  document.addEventListener('keydown', e => { if (e.key === 'Escape') closeQuickView(); });
}

async function openQuickView(id) {
  ensureQuickView();
  const body = document.getElementById('qvBody');
  body.innerHTML = '<div class="loading">Đang tải…</div>';
  document.getElementById('qvOverlay').classList.add('open');
  document.getElementById('qvDrawer').classList.add('open');
  document.body.style.overflow = 'hidden';
  qvQty = 1;
  try {
    const p = await api(`/products/${id}`);
    const out = p.stock <= 0;
    body.innerHTML = `
      <div class="qv-media">${productImage(p)}</div>
      <div class="qv-content">
        <div class="eyebrow">${p.categoryName || 'Nước hoa'}</div>
        <h2>${p.name}</h2>
        <div class="detail-brand">${p.brand || ''}</div>
        <div class="qv-price">${formatVND(p.price)}</div>
        ${p.notes ? `<div class="detail-notes">${noteTags(p.notes)}</div>` : ''}
        <p class="qv-desc">${p.description || 'Chưa có mô tả cho sản phẩm này.'}</p>
        <div class="qv-actions">
          <div class="qty">
            <button onclick="qvChangeQty(-1)">−</button>
            <span id="qvQty">1</span>
            <button onclick="qvChangeQty(1)">+</button>
          </div>
          <button class="btn btn-gold" style="flex:1" ${out ? 'disabled' : ''} onclick="qvAdd(${p.id})">
            ${out ? 'Hết hàng' : 'Thêm vào giỏ'}
          </button>
        </div>
        <a href="product.html?id=${p.id}" class="qv-full-link">Xem chi tiết đầy đủ →</a>
      </div>`;
  } catch (e) {
    body.innerHTML = `<div class="empty-state">${e.message}</div>`;
  }
}

function qvChangeQty(d) {
  qvQty = Math.max(1, qvQty + d);
  document.getElementById('qvQty').textContent = qvQty;
}

async function qvAdd(productId) {
  if (!Auth.isLoggedIn) {
    toast('Vui lòng đăng nhập để mua hàng.');
    setTimeout(() => location.href = 'account.html', 900);
    return;
  }
  try {
    await api('/cart/items', { method: 'POST', auth: true, body: { productId, quantity: qvQty } });
    toast('Đã thêm vào giỏ hàng ✓');
    updateCartCount();
  } catch (e) { toast(e.message); }
}

function closeQuickView() {
  document.getElementById('qvOverlay')?.classList.remove('open');
  document.getElementById('qvDrawer')?.classList.remove('open');
  document.body.style.overflow = '';
}

/* ===========================================================
   Hiệu ứng cuộn (scroll animations)
   =========================================================== */
const prefersReducedMotion =
  window.matchMedia && window.matchMedia('(prefers-reduced-motion: reduce)').matches;

// Hiện dần các phần tử có class .reveal khi cuộn tới
function initReveal(root = document) {
  const els = root.querySelectorAll('.reveal:not(.in), .reveal-fade:not(.in)');
  if (prefersReducedMotion || !('IntersectionObserver' in window)) {
    els.forEach(e => e.classList.add('in'));
    return;
  }
  const io = new IntersectionObserver((entries, obs) => {
    entries.forEach(entry => {
      if (entry.isIntersecting) {
        entry.target.classList.add('in');
        obs.unobserve(entry.target);
      }
    });
  }, { threshold: 0.12, rootMargin: '0px 0px -8% 0px' });
  els.forEach(e => io.observe(e));
}

// Parallax nhẹ cho khu vực hero: nội dung trôi chậm và mờ dần khi cuộn
function initParallax() {
  if (prefersReducedMotion) return;
  const content = document.querySelector('.hero-content');
  if (!content) return;
  const vh = window.innerHeight;
  window.addEventListener('scroll', () => {
    const y = window.scrollY;
    if (y <= vh) {
      content.style.transform = `translateY(${y * 0.25}px)`;
      content.style.opacity = String(Math.max(0, 1 - y / (vh * 0.9)));
    }
  }, { passive: true });
}

// Header thêm bóng + thu gọn khi cuộn xuống
function initHeaderScroll() {
  const onScroll = () => {
    const header = document.querySelector('.site-header');
    if (header) header.classList.toggle('scrolled', window.scrollY > 20);
  };
  window.addEventListener('scroll', onScroll, { passive: true });
  onScroll();
}

// Fade nhẹ khi chuyển sang trang nội bộ (.html)
function initPageTransitions() {
  if (prefersReducedMotion) return;
  document.addEventListener('click', (e) => {
    const a = e.target.closest('a');
    if (!a) return;
    const href = a.getAttribute('href');
    if (!href) return;
    // Bỏ qua link ngoài, neo, mailto/tel; chỉ xử lý trang nội bộ .html
    if (href.startsWith('http') || href.startsWith('#') ||
        href.startsWith('mailto') || href.startsWith('tel')) return;
    if (!href.includes('.html')) return;
    if (a.target === '_blank' || e.metaKey || e.ctrlKey) return;
    e.preventDefault();
    document.body.classList.add('page-out');
    setTimeout(() => { location.href = href; }, 260);
  });
}

// Hiệu ứng chữ xuất hiện theo từng từ (text reveal)
function initTextReveal() {
  const els = document.querySelectorAll('.text-reveal:not(.tr-ready)');
  els.forEach(el => {
    el.classList.add('tr-ready');
    const words = el.textContent.trim().split(/\s+/);
    el.innerHTML = words.map((w, i) =>
      `<span class="tr-word"><span class="tr-inner" style="transition-delay:${i * 70}ms">${w}</span></span>`
    ).join(' ');
  });
  if (prefersReducedMotion || !('IntersectionObserver' in window)) {
    document.querySelectorAll('.text-reveal').forEach(e => e.classList.add('in'));
    return;
  }
  const io = new IntersectionObserver((entries, obs) => {
    entries.forEach(en => {
      if (en.isIntersecting) { en.target.classList.add('in'); obs.unobserve(en.target); }
    });
  }, { threshold: 0.2 });
  document.querySelectorAll('.text-reveal').forEach(e => io.observe(e));
}

// Tự chạy khi trang tải xong
window.addEventListener('DOMContentLoaded', () => {
  initReveal();
  initTextReveal();
  initParallax();
  initHeaderScroll();
  initPageTransitions();
});

/* ---------- Cập nhật số lượng trên biểu tượng giỏ ---------- */
async function updateCartCount() {
  const badge = document.getElementById('cartCount');
  if (!badge || !Auth.isLoggedIn) return;
  try {
    const cart = await api('/cart', { auth: true });
    const count = cart.items.reduce((s, i) => s + i.quantity, 0);
    if (count > 0) { badge.textContent = count; badge.classList.remove('hidden'); }
    else badge.classList.add('hidden');
  } catch { /* im lặng nếu chưa đăng nhập */ }
}
