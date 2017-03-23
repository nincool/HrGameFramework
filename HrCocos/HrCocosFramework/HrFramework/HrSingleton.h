#ifndef _HR_SINGLETON_H_
#define _HR_SINGLETON_H_

#include <memory>

template <typename T>
class HrSingleTon
{
public:
	HrSingleTon() {};

	static std::unique_ptr<T>& Instance();
	static void Release();
protected:
	virtual ~HrSingleTon() {};
	HrSingleTon(const HrSingleTon&) = delete;
	HrSingleTon& operator=(const HrSingleTon&) = delete;

private:
	static std::unique_ptr<T> m_pInstancePtr;
};

template <typename T>
std::unique_ptr<T> HrSingleTon<T>::m_pInstancePtr;


template <typename T>
std::unique_ptr<T>& HrSingleTon<T>::Instance()
{
	if (!m_pInstancePtr)
	{
		m_pInstancePtr.reset(new T());
	}
	return m_pInstancePtr;
}

template <typename T>
void HrSingleTon<T>::Release()
{
	if (m_pInstancePtr)
	{
		m_pInstancePtr.reset(nullptr);
	}
}

#endif


